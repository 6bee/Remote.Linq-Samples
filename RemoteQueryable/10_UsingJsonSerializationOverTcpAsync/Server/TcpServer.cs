﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Server
{
    using Common;
    using Remote.Linq.Expressions;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class TcpServer : IDisposable
    {
        private readonly TcpListener _server;

        public TcpServer(int port)
            : this("127.0.0.1", port)
        {
        }

        public TcpServer(string ip, int port)
        {
            var ipAddress = IPAddress.Parse(ip);
            _server = new TcpListener(ipAddress, port);
        }

        public void Open()
        {
            _server.Start();

            Task.Run(async () =>
            {
                while (true)
                {
                    using (var client = _server.AcceptTcpClient())
                    {
                        using (var stream = client.GetStream())
                        {
                            var queryExpression = await stream.ReadAsync<Expression>().ConfigureAwait(false);

                            try
                            {
                                var queryService = new QueryService();
                                var result = await queryService.ExecuteQueryAsync(queryExpression);
                                await stream.WriteAsync(result).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                await stream.WriteAsync(ex).ConfigureAwait(false);
                            }

                            stream.Close();
                        }

                        client.Close();
                    }
                }
            });
        }

        public void Dispose()
        {
            try
            {
                _server.Stop();
            }
            catch
            {
            }
        }
    }
}
