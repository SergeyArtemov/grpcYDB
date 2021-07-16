
using System;
using Grpc.Core;
using System.Threading.Tasks;
using Yandex.Cloud.Credentials;
using Yandex.Cloud;
using Yandex.Cloud.Resourcemanager.V1;
using Ydb.Cms;
using Ydb.Coordination;
using Ydb.Table;
using Ydb.Table.V1;
using System.Text;

namespace grpsApp1
{
    class Program
    {

        static async Task Main(string[] args)
        {

            // Sdk Cloud РАБОТАЕТ!!!
            Sdk _sdk = new Sdk(new OAuthCredentialsProvider("AQAAAAAAnb4MAATuwVBy2WSTL0EPjlNvh1ZN5tA"));
            var client1 = _sdk.Services.Resourcemanager.CloudService;
            Console.WriteLine("ListOperations=" + client1.ToString());
            //Console.WriteLine("ListOperations=" + client1.ListOperations().ToString);
            var resp1 = client1.List(new ListCloudsRequest());
            Console.WriteLine(resp1.Clouds.ToString());
            //-------------------------------

            // Попытаемся чатсично воспроизвести поведение SDK Cloud
             // РАБОТАЕТ!!!
            Sdk _sdk10 = new Sdk(new OAuthCredentialsProvider("AQAAAAAAnb4MAATuwVBy2WSTL0EPjlNvh1ZN5tA"));
            // Получаем объект ChannelCredentials
            var cc10 = _sdk10.GetCredentials();
            // Получаем Channel
            Channel channel10 = new Channel("resource-manager.api.cloud.yandex.net:443",/*ChannelCredentials.Insecure*/ cc10);
            var client10 = new CloudService.CloudServiceClient(channel10);
            var resp4 = client10.List(new ListCloudsRequest());
            Console.WriteLine("^^^"+client10.ToString());
            Console.WriteLine("$$$"+resp4.ToString());

            //-------------------------------------------------

            // Попытаемся полонстью воспроизвести поведение SDK Cloud
            // Полностью обходимся без SDK Cloud!!!
            // РАБОТАЕТ!!!
            ChannelCredentials channelCredentials11 = new SslCredentials();
            var cc11 =
            ChannelCredentials.Create(
                channelCredentials11,
                CallCredentials.FromInterceptor(async (context, metadata) =>   // Передаём в делегат
                                                                               // delegate Task AsyncAuthInterceptor(AuthInterceptorContext context, Metadata metadata)
                                                                               // ссылочную переменную context (без объекта)
                                                                               // и уже созданный и наполненный объект metadata.

                {
                    metadata.Add(
                        new Metadata.Entry(
                            "authorization",
                            $"Bearer {new OAuthCredentialsProvider("AQAAAAAAnb4MAATuwVBy2WSTL0EPjlNvh1ZN5tA").GetToken()}"
                        )
                    );
                })
            );

            Channel channel11 = new Channel("resource-manager.api.cloud.yandex.net:443", cc11);
            var client11 = new CloudService.CloudServiceClient(channel11);
            var resp11 = client11.List(new ListCloudsRequest());
            Console.WriteLine("*****"+resp11.ToString());

            //-------------------------------------------------

            // Теперь попробуем аналогичным образом обратиться к YDB
            // НЕ РАБОТАЕТ!!!
            //Ошибка:
            //Grpc.Core.RpcException: "Status(StatusCode="Unavailable"
            //, Detail="failed to connect to all addresses"
            //, DebugException="Grpc.Core.Internal.CoreErrorDetailException
            //: { "created":"@1620137115.861000000","description":"Failed to pick subchannel"
            //,"file":"..\..\..\src\core\ext\filters\client_channel\client_channel.cc"


            //,"file_line":5420,"referenced_errors"
            //:[{ "created":"@1620137115.861000000"
            //,"description":"failed to connect to all addresses"
            //,"file":"..\..\..\src\core\ext\filters\client_channel\lb_policy\pick_first\pick_first.cc","file_line":398,"grpc_status":14}]}
            //")"

            //Sdk _sdk15 = new Sdk(new OAuthCredentialsProvider("AQAAAAAAnb4MAATuwVBy2WSTL0EPjlNvh1ZN5tA"));
            // Получаем объект ChannelCredentials


            ChannelCredentials channelCredentials1 = new SslCredentials(
                //string.Concat()
                /*File.ReadAllText("C:/Cert/CA.pem")*/
                );
            
            var cc15 =
            ChannelCredentials.Create(
                channelCredentials1,
                CallCredentials.FromInterceptor(async (context, metadata) =>
                {
                    metadata.Add(
                        new Metadata.Entry(
                            "authorization",
                            $"Bearer {new OAuthCredentialsProvider("AQAAAAAAnb4MAATuwVBy2WSTL0EPjlNvh1ZN5tA").GetToken()}"
                        )
                    );

                    metadata.Add(new Metadata.Entry("Database", "ru-central1/b1gkdp1j3vovv5gh0ajr/etn00ckc408gskmlgqqb"));
                    //   metadata.Add(
                    //       new Metadata.Entry(
                    //           "grpc.enable_http_proxy",
                    //           "0"
                    //       )
                    //   );
                })
            );


            //CallOptions opts = new Grpc.Core.CallOptions();
            //opts.W

            //var md = new Metadata();
            //md.Add("grpc.enable_http_proxy", "0");

            //Channel channel15 = new Channel("ydb.serverless.yandexcloud.net:2135", cc15);
            //var client15 = new grpsApp1.DatabaseService.DatabaseServiceClient(channel15);
            //ListDatabasesRequest ldbs = new ListDatabasesRequest();
            //ldbs.FolderId = "b1gtdtrvh7ardms3vmpl";
            //var resp15 = client15.List(ldbs/*,new CallOptions(md)*/);

            Channel channel15 = new Channel("ydb.serverless.yandexcloud.net:2135", cc15);
            //channel15.
            //var cc = channel15.State;
            var pp1 = channel15.ConnectAsync();
            var pp2 = channel15.State;//TryWaitForStateChangedAsync(Grpc.Core.ChannelState.Idle);
            Console.WriteLine("PP" + pp2.ToString());
            //var pp2 = channel15.TryWaitForStateChangedAsync(Grpc.Core.ChannelState.Connecting);
            await channel15.WaitForStateChangedAsync(ChannelState.Connecting);
            pp2 = channel15.State;
            Console.WriteLine("PP" + pp2.ToString());
            await channel15.WaitForStateChangedAsync(ChannelState.TransientFailure);
            pp2 = channel15.State;
            Console.WriteLine("PP" + pp2.ToString());
            await channel15.WaitForStateChangedAsync(ChannelState.Connecting);
            pp2 = channel15.State;
            Console.WriteLine("PP" + pp2.ToString());


            await channel15.WaitForStateChangedAsync(ChannelState.TransientFailure);
            pp2 = channel15.State;
            Console.WriteLine("PP" + pp2.ToString());
            await channel15.WaitForStateChangedAsync(ChannelState.Connecting);
            pp2 = channel15.State;
            Console.WriteLine("PP" + pp2.ToString());

            await channel15.WaitForStateChangedAsync(ChannelState.TransientFailure);
            pp2 = channel15.State;
            Console.WriteLine("PP" + pp2.ToString());
            await channel15.WaitForStateChangedAsync(ChannelState.Connecting);
            pp2 = channel15.State;
            Console.WriteLine("PP" + pp2.ToString());

            await channel15.WaitForStateChangedAsync(ChannelState.TransientFailure);
            pp2 = channel15.State;
            Console.WriteLine("PP" + pp2.ToString());
            await channel15.WaitForStateChangedAsync(ChannelState.Connecting);
            pp2 = channel15.State;
            Console.WriteLine("PP" + pp2.ToString());

            await channel15.WaitForStateChangedAsync(ChannelState.TransientFailure);
            pp2 = channel15.State;
            Console.WriteLine("PP" + pp2.ToString());
            await channel15.WaitForStateChangedAsync(ChannelState.Connecting);
            pp2 = channel15.State;
            Console.WriteLine("PP" + pp2.ToString());


            await channel15.WaitForStateChangedAsync(ChannelState.TransientFailure);
            pp2 = channel15.State;
            Console.WriteLine("PP" + pp2.ToString());
            await channel15.WaitForStateChangedAsync(ChannelState.Connecting);
            pp2 = channel15.State;
            Console.WriteLine("PP" + pp2.ToString());

            await channel15.WaitForStateChangedAsync(ChannelState.TransientFailure);
            pp2 = channel15.State;
            Console.WriteLine("PP" + pp2.ToString());
            await channel15.WaitForStateChangedAsync(ChannelState.Connecting);
            pp2 = channel15.State;
            Console.WriteLine("PP" + pp2.ToString());


            //var client15 = new DatabaseService.DatabaseServiceClient(channel15);

            //var client15 = new Database(//DatabaseService.DatabaseServiceClient(channel15);

            //GetDatabaseStatusRequest h2 = new Ydb.Cms.GetDatabaseStatusRequest();
            //h2.OperationParams.;
            //var h2 = sess1.

            //Ydb.Table.BeginTransactionRequest h33 = new Ydb.Table.BeginTransactionRequest()

            //var sreq = new Ydb.Coordination.SessionRequest();//.SessionStartFieldNumber();
            //var sreq1 = new Ydb.Table.CreateTableRequest(new CreateTableRequest());


            //var sess1 = new Ydb.Table.CreateSessionRequest();
            //sess1.

            //Поcледовательность:https://cloud.yandex.ru/docs/ydb/sdk/   А ПРИМЕРЫ на JAVA ПРОЩЕ!!
            //1.Драйвер.
            //См.   https://github.com/yandex-cloud/ydb-go-sdk/blob/master/table/session.go  поиск по ydb.Driver
            //      https://github.com/yandex-cloud/ydb-go-sdk/blob/master/driver.go
            //      https://github.com/yandex-cloud/ydb-go-sdk/blob/master/dialer.go
            //2.Клиент.
            //3.Сессия.
            // 

            //Реализация:
            //1.Драйвер.

            //2.Клиент.
            // Создали клиента на основе канала

            var cttab = new Ydb.Table.V1.TableService.TableServiceClient(channel15);

            //cttab.databa

            //var cttab2 = new cttab.CreateSession
            //cttab.
            //cttab.
            //Console.WriteLine("JJJ" + cttab.);
            //.....
            //var pppp1 = new Ydb.Discovery.ListEndpointsRequest();
            //pppp1.Database = "ru-central1/b1gkdp1j3vovv5gh0ajr/etn00ckc408gskmlgqqb";
            //Google.Protobuf.CodedOutputStream cos;

            var war = new Ydb.Discovery.V1.DiscoveryService.DiscoveryServiceClient(channel15);
            var pppp1 = new Ydb.Discovery.ListEndpointsRequest();
            pppp1.Database = "ru-central1/b1gkdp1j3vovv5gh0ajr/etn00ckc408gskmlgqqb";
            //pppp1.Service = "ru-central1/b1gkdp1j3vovv5gh0ajr/etn00ckc408gskmlgqqb";
            var reswar = war.ListEndpoints(pppp1);
            //var war = new Ydb.Discovery.
            Console.WriteLine("MMM" + reswar.ToString());

            //3.Сессия.
            // Создали сессию
            //var mes_param = new Ydb.Operations.OperationParams();
            //mes_param.

            var sessreq1 = new Ydb.Table.CreateSessionRequest(); 
            //sessreq1.OperationParams.OperationMode
            //{ OperationParams = "ru-central1/b1gkdp1j3vovv5gh0ajr/etn00ckc408gskmlgqqb" };
            //var op = new Ydb.Operations.OperationParams();
            //op.
            
            //sessreq1.OperationParams = op;

            var metad = new Metadata();
            metad.Add(new Metadata.Entry("endpoint", "ydb.serverless.yandexcloud.net:2135"));
            //metad.Add(new Metadata.Entry(Utf16ToUtf8("Database"), "ru-central1/b1gkdp1j3vovv5gh0ajr/etn00ckc408gskmlgqqb"));
            metad.Add(new Metadata.Entry(Utf16ToUtf8("Database"), "ru-central1/b1gkdp1j3vovv5gh0ajr/etn00ckc408gskmlgqqb"));
            metad.Add(new Metadata.Entry("requestsType", "dsdsdsd"));
            metad.Add(new Metadata.Entry("token", "sasasasa"));
            metad.Add(new Metadata.Entry("DB", "sasasasa"));
            metad.Add(new Metadata.Entry("ID", "sasasasa"));

            //var callop = new CallOptions();
            //callop

            var co = new CallOptions();
            var co1 = co.WithHeaders(metad);

            var sess1 = cttab.CreateSession( sessreq1, co1);//, new CallOptions { Headers = "ru-central1/b1gkdp1j3vovv5gh0ajr/etn00ckc408gskmlgqqb" }); //;//new CallOptions().WithHeaders(metad));   // database_id = ... см.:  ydb_experimental.proto
            //sess1
            
            Console.WriteLine("SSS" + sess1.ToString());


            //4.Работаем с таблицами.
            var metad2 = new Metadata();
            metad2.Add(new Metadata.Entry("table", "ru-central1/b1gkdp1j3vovv5gh0ajr/etn00ckc408gskmlgqqb/table1"));

            var dts = new DescribeTableRequest(); // {  = "ru-central1/b1gkdp1j3vovv5gh0ajr/etn00ckc408gskmlgqqb/table1" };
            
            var respp1 = cttab.DescribeTable(dts, new CallOptions().WithHeaders(metad2));
            Console.WriteLine("NN" + respp1.ToString());



            //-------------------------
            //Console.WriteLine("DDDstart");
            //var dbsev2 = new Ydb.Cms.V1.CmsService.CmsServiceClient(channel15);
            //var dbreq2 = new Ydb.Cms.ListDatabasesRequest();//ListDatabaseRequest();
            //var dbres2 = dbsev2.ListDatabases(dbreq2);
            //Console.WriteLine("DDD" + dbres2.ToString());

            //.V1.TableService.TableServiceClient(channel15);

            //ChannelCredentials channelCredentials16 = new SslCredentials(File.ReadAllText("C:/Cert/CA.pem"));

            /*
            var g = new GetDatabaseRequest();
            g.DatabaseId = "ru-central1/b1gkdp1j3vovv5gh0ajr/etn00ckc408gskmlgqqb";
            //var resp15 = client15.Get(g); //etn00ckc408gskmlgqqb
            var resp15 = await client15.GetAsync(g, new CallOptions().WithCredentials(
                                    CallCredentials.FromInterceptor(async (context, metadata) =>
                                    {
                                        metadata.Add(
                                            new Metadata.Entry(
                                                "authorization",
                                                $"Bearer {new OAuthCredentialsProvider("AQAAAAAAnb4MAATuwVBy2WSTL0EPjlNvh1ZN5tA").GetToken()}"
                                            )
                                        );

                                       // metadata.Add(
                                       //     new Metadata.Entry(
                                       //         "grpc.enable_http_proxy",
                                       //         "0"
                                       //     )
                                       // );
                                    })

                ));
            */

            //var ldb = new ListDatabasesRequest();
            //var resp16 = client15.List(ldb);

            //Console.WriteLine("NN" + resp16.ToString());

            // metadata.Add(
            //     new Metadata.Entry(
            //         "grpc.enable_http_proxy",
            //         "0"
            //     )
            // );


            /*
            var resp15 = client15.List(new ListDatabasesRequest(),new CallOptions().WithCredentials(
                                    CallCredentials.FromInterceptor(async (context, metadata) =>
                                    {
                                        metadata.Add(
                                            new Metadata.Entry(
                                                "authorization",
                                                $"Bearer {new OAuthCredentialsProvider("AQAAAAAAnb4MAATuwVBy2WSTL0EPjlNvh1ZN5tA").GetToken()}"
                                            )
                                        );

                                           metadata.Add(
                                               new Metadata.Entry(
                                                   "grpc.enable_http_proxy",
                                                   "0"
                                               )
                                           );
                                    })

                ));*/
            //Console.WriteLine("TTTTT"+resp15.ToString());

        }

        public static string Utf16ToUtf8(string utf16String)
        {
            /**************************************************************
             * Every .NET string will store text with the UTF16 encoding, *
             * known as Encoding.Unicode. Other encodings may exist as    *
             * Byte-Array or incorrectly stored with the UTF16 encoding.  *
             *                                                            *
             * UTF8 = 1 bytes per char                                    *
             *    ["100" for the ansi 'd']                                *
             *    ["206" and "186" for the russian 'κ']                   *
             *                                                            *
             * UTF16 = 2 bytes per char                                   *
             *    ["100, 0" for the ansi 'd']                             *
             *    ["186, 3" for the russian 'κ']                          *
             *                                                            *
             * UTF8 inside UTF16                                          *
             *    ["100, 0" for the ansi 'd']                             *
             *    ["206, 0" and "186, 0" for the russian 'κ']             *
             *                                                            *
             * We can use the convert encoding function to convert an     *
             * UTF16 Byte-Array to an UTF8 Byte-Array. When we use UTF8   *
             * encoding to string method now, we will get a UTF16 string. *
             *                                                            *
             * So we imitate UTF16 by filling the second byte of a char   *
             * with a 0 byte (binary 0) while creating the string.        *
             **************************************************************/

            // Storage for the UTF8 string
            string utf8String = String.Empty;

            // Get UTF16 bytes and convert UTF16 bytes to UTF8 bytes
            byte[] utf16Bytes = Encoding.Unicode.GetBytes(utf16String);
            byte[] utf8Bytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, utf16Bytes);

            // Fill UTF8 bytes inside UTF8 string
            for (int i = 0; i < utf8Bytes.Length; i++)
            {
                // Because char always saves 2 bytes, fill char with 0
                byte[] utf8Container = new byte[2] { utf8Bytes[i], 0 };
                utf8String += BitConverter.ToChar(utf8Container, 0);
            }

            // Return UTF8
            return utf8String;
        }

    }
}
