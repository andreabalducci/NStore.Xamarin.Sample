using System;
using System.IO;
using System.Threading;
using NStore.Core.Persistence;
using NStore.Core.Streams;
using NStore.Domain;
using NStore.Persistence.Sqlite;

namespace NStoreSample.Engine
{
    public interface IAppEngine
    {
        IRepository GetRepository();
    }

    public class AppEngine : IAppEngine
    {
        private IStreamsFactory _streams;
        private IAggregateFactory _aggregateFactory = new DefaultAggregateFactory();
       
        public AppEngine()
        {
            var pathToDb = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "sample.db");

            if (File.Exists(pathToDb))
                File.Delete(pathToDb);

            var options = new SqlitePersistenceOptions(new NStore.Core.Logging.NStoreNullLoggerFactory())
            {
                ConnectionString = $"Data Source={pathToDb}",
                Serializer = new TypeAsSchemaJsonSerializer()
            };
            var persitence = new SqlitePersistence(options);
            _streams = new StreamsFactory(persitence);

            SQLitePCL.Batteries_V2.Init();

            persitence.InitAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        public IRepository GetRepository()
        {
            return new Repository(
                _aggregateFactory,
                _streams
            );
        }
    }
}
