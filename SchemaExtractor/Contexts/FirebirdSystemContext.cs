using FirebirdSql.Data.FirebirdClient;
using System.Data.Entity;
using JetBrains.Annotations;
using SchemaExtractor.Contexts;
using SchemaExtractor.Entities;

namespace SchemaExtractor
{
    public class FirebirdSystemContext : DbContext
    {
        public FirebirdSystemContext(string connectionString) : base(new FbConnection(connectionString), true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Add(new FirebirdSystemFieldConvention());

            modelBuilder.Entity<Procedure>()
                .ToTable("RDB$PROCEDURES")
                .HasKey(a => a.RdbProcedureName);

            modelBuilder.Entity<ProcedureParameter>()
                .ToTable("RDB$PROCEDURE_PARAMETERS")
                .HasKey(a => new {a.RdbProcedureName, a.RdbParameterName})
                .HasRequired(s => s.RdbField)
                    .WithMany()
                    .HasForeignKey(a => a.RdbFieldSource);

            modelBuilder.Entity<Field>()
                .ToTable("RDB$FIELDS")
                .HasKey(a => a.RdbFieldName);

            modelBuilder.Entity<Function>()
                .ToTable("RDB$FUNCTIONS")
                .HasKey(a => a.RdbFunctionName);

            modelBuilder.Entity<FunctionArgument>()
                .ToTable("RDB$FUNCTION_ARGUMENTS")
                .HasKey(a => new { a.RdbFunctionName, a.RdbArgumentPosition })
                .HasRequired(a => a.RdbFunction)
                    .WithMany(a => a.RdbFunctionArguments)
                    .HasForeignKey(a => a.RdbFunctionName);

            modelBuilder.Entity<Relation>()
                .ToTable("RDB$RELATIONS")
                .HasKey(a => a.RdbRelationName);

            modelBuilder.Entity<RelationField>()
                .ToTable("RDB$RELATION_FIELDS")
                .HasKey(a => new { a.RdbRelationName, a.RdbFieldName });

            modelBuilder.Entity<RelationField>()
                .HasRequired(a => a.RdbRelation)
                    .WithMany(a => a.RdbRelationFields)
                    .HasForeignKey(a => a.RdbRelationName);

            modelBuilder.Entity<RelationField>()
                .HasRequired(a => a.RdbField)
                    .WithMany()
                    .HasForeignKey(a => a.RdbFieldSource);

            modelBuilder.Entity<Trigger>()
                .ToTable("RDB$TRIGGERS")
                .HasKey(a => a.RdbTriggerName)
                .HasRequired(a => a.RdbRelation)
                    .WithMany(a => a.RdbTriggers)
                    .HasForeignKey(a => a.RdbRelationName);

            modelBuilder.Entity<UserPrivilege>()
                .ToTable("RDB$USER_PRIVILEGES")
                .HasKey(a => new {a.RdbRelationName, a.RdbPrivilege, a.RdbUser});
                
            modelBuilder.Entity<Dependency>()
                .ToTable("RDB$DEPENDENCIES")
                .HasKey(a => new {a.RdbDependentName, a.RdbDependedOnName, a.RdbFieldName});

            modelBuilder.Entity<Collation>()
                .ToTable("RDB$COLLATIONS")
                .HasKey(a => new { a.RdbCollationId, a.RdbCharacterSetId });

            modelBuilder.Entity<CharacterSet>()
                .ToTable("RDB$CHARACTER_SETS")
                .HasKey(a => a.RdbCharacterSetId);

            modelBuilder.Entity<RelationConstraint>()
                .ToTable("RDB$RELATION_CONSTRAINTS")
                .HasKey(a => a.RdbConstraintName);

            modelBuilder.Entity<RelationConstraint>()
                .HasRequired(a => a.RdbRelation)
                    .WithMany(a => a.RdbRelationConstraint)
                    .HasForeignKey(a => a.RdbRelationName);

            // 1 to 1 RdbRelationConstraint.RdbIndexName => RdbIndex.RdbIndexName
            modelBuilder.Entity<RelationConstraint>()
                .HasOptional(a => a.RdbIndex)
                    .WithMany()
                    .HasForeignKey(u => u.RdbIndexName);

            modelBuilder.Entity<Index>()
                .ToTable("RDB$INDICES")
                .HasKey(a => a.RdbIndexName);

            modelBuilder.Entity<Index>()
                .HasRequired(index => index.RdbRelation)
                .WithMany(relation => relation.RdbIndices)
                .HasForeignKey(index => index.RdbRelationName);

            modelBuilder.Entity<IndexSegment>()
                .ToTable("RDB$INDEX_SEGMENTS")
                .HasKey(a => new { a.RdbIndexName, a.RdbFieldName, a.RdbFieldPosition });
            
            modelBuilder.Entity<IndexSegment>()
                .HasRequired(a => a.RdbIndex)
                    .WithMany(a => a.RdbIndexSegments)
                    .HasForeignKey(a => a.RdbIndexName);
        }
        public DbSet<Procedure> RdbProcedures { get; [UsedImplicitly] set; }
        public DbSet<Function> RdbFunctions { get; [UsedImplicitly] set; }
        public DbSet<Relation> RdbRelations { get; [UsedImplicitly] set; }
        public DbSet<UserPrivilege> RdbUserPrivileges { get; [UsedImplicitly] set; }
        public DbSet<Dependency> RdbDependencies { get; [UsedImplicitly] set; }
        public DbSet<Collation> RdbCollations { get; [UsedImplicitly] set; }
        public DbSet<CharacterSet> RdbCharacterSets { get; [UsedImplicitly] set; }
        public DbSet<RelationConstraint> RdbRelationConstraints { get; [UsedImplicitly] set; }
        public DbSet<IndexSegment> RdbIndexSegments { get; [UsedImplicitly] set; }
        public DbSet<Index> RdbIndices { get; [UsedImplicitly] set; }
        public DbSet<Trigger> RdbTriggers { get; [UsedImplicitly] set; }
    }
}
