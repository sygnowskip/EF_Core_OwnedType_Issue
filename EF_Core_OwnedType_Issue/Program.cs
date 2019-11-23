using System.Linq;
using DbUp;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace EF_Core_OwnedType_Issue
{
    class Program
    {
        //Execute 'docker-compose up' in main directory or provide your own database connection string
        const string ConnectionString = "Data Source=127.0.0.1,1700;Initial Catalog=master;Connection Timeout=30;User Id=sa;Password=P@ssword1;MultipleActiveResultSets=False;Timeout=30;";
        private static string DatabaseConnectionString => ConnectionString.Replace("master", "Candidates");

        static void Main(string[] args)
        {
            PrepareTableSchema();

            var builder = new DbContextOptionsBuilder<CandidateDbContext>();
            builder.UseSqlServer(DatabaseConnectionString);

            WorkingUpdateOnNonNullOwnedType(builder.Options);

            NotWorkingUpdateOnNullOwnedType(builder.Options);
        }

        private static void NotWorkingUpdateOnNullOwnedType(DbContextOptions<CandidateDbContext> builderOptions)
        {
            long? candidateId = null;
            using (var dbContext = new CandidateDbContext(builderOptions))
            {
                var candidate = new Candidate()
                {
                    FirstName = "FirstName"
                };

                dbContext.Add(candidate);
                dbContext.SaveChanges();
                candidateId = candidate.Id;
            }

            using (var dbContext = new CandidateDbContext(builderOptions))
            {
                var candidate = dbContext.Candidates.SingleOrDefault(c => c.Id == candidateId);

                if (candidate != null)
                {
                    candidate.Address = new Address()
                    {
                        Street = "Street2",
                        PostalCode = "Postal2"
                    };
                    dbContext.SaveChanges();
                }
            }
        }

        private static void WorkingUpdateOnNonNullOwnedType(DbContextOptions<CandidateDbContext> builderOptions)
        {
            long? candidateId = null;
            using (var dbContext = new CandidateDbContext(builderOptions))
            {
                var candidate = new Candidate()
                {
                    FirstName = "FirstName",
                    Address = new Address()
                    {
                        Street = "Street",
                        PostalCode = "Postal"
                    }
                };

                dbContext.Add(candidate);
                dbContext.SaveChanges();
                candidateId = candidate.Id;
            }

            using (var dbContext = new CandidateDbContext(builderOptions))
            {
                var candidate = dbContext.Candidates.SingleOrDefault(c => c.Id == candidateId);

                if (candidate != null)
                {
                    candidate.Address = new Address()
                    {
                        Street = "Street2",
                        PostalCode = "Postal2"
                    };
                    dbContext.SaveChanges();
                }
            }
        }


        private static void PrepareTableSchema()
        {
            DeployChanges.To
                .SqlDatabase(ConnectionString)
                .WithScriptsEmbeddedInAssembly(typeof(Program).Assembly)
                .Build()
                .PerformUpgrade();
        }
    }
}
