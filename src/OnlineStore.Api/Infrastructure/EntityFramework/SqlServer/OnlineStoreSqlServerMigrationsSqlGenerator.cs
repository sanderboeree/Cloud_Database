using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Linq;

namespace OnlineStore.Api.Infrastructure.EntityFramework.SqlServer
{
    public class OnlineStoreSqlServerMigrationsSqlGenerator : SqlServerMigrationsSqlGenerator
    {
        public OnlineStoreSqlServerMigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, IRelationalAnnotationProvider migrationsAnnotations) : base(dependencies, migrationsAnnotations)
        {

        }

        protected override void Generate(CreateTableOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
        {
            base.Generate(operation, model, builder, terminate);

            foreach (var columnOperation in operation.Columns.Where(columnOperation => columnOperation.FindAnnotation(OnlineStoreAnnotation.ModifiedTrigger) != null))
            {
                CreateModifiedTrigger(columnOperation.Schema, columnOperation.Table, columnOperation.Name, builder);
            }
        }

        protected override void Generate(AddColumnOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate)
        {
            base.Generate(operation, model, builder, terminate);

            if (operation.FindAnnotation(OnlineStoreAnnotation.ModifiedTrigger) != null)
            {
                CreateModifiedTrigger(operation.Schema, operation.Table, operation.Name, builder);
            }
        }

        protected override void Generate(AlterColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            base.Generate(operation, model, builder);

            var oldColumnHasAnnotation = operation.OldColumn.FindAnnotation(OnlineStoreAnnotation.ModifiedTrigger) != null;
            var newColumnHasAnnotation = operation.FindAnnotation(OnlineStoreAnnotation.ModifiedTrigger) != null;

            if (oldColumnHasAnnotation && !newColumnHasAnnotation)
            {
                DropModifiedTrigger(operation.Schema, operation.Table, operation.Name, builder);
            }

            if (!oldColumnHasAnnotation && newColumnHasAnnotation)
            {
                CreateModifiedTrigger(operation.Schema, operation.Table, operation.Name, builder);
            }
        }

        protected override void Generate(DropColumnOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
        {
            if (operation.FindAnnotation(OnlineStoreAnnotation.ModifiedTrigger) != null)
            {
                DropModifiedTrigger(operation.Schema, operation.Table, operation.Name, builder);
            }

            base.Generate(operation, model, builder, terminate);
        }

        private static void CreateModifiedTrigger(string schema, string tableName, string columnName, MigrationCommandListBuilder builder)
        {
            if (string.IsNullOrWhiteSpace(schema))
            {
                schema = "dbo";
            }

            builder
                .AppendLine($"CREATE TRIGGER [{schema}].[TR_{tableName}_{columnName}] ON [{schema}].[{tableName}] AFTER UPDATE AS " +
                            $"UPDATE [{schema}].[{tableName}] SET {columnName} = GETUTCDATE() FROM Inserted WHERE [{schema}].[{tableName}].Id = Inserted.Id")
                .EndCommand();
        }

        private static void DropModifiedTrigger(string schema, string tableName, string columnName, MigrationCommandListBuilder builder)
        {
            if (string.IsNullOrWhiteSpace(schema))
            {
                schema = "dbo";
            }

            builder
                .AppendLine($"DROP TRIGGER [{schema}].[TR_{tableName}_{columnName}]")
                .EndCommand();
        }
    }
}
