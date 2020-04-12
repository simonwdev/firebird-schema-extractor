using System.IO;
using System.Linq;
using SchemaExtractor.ReferenceTypes;
using SchemaExtractor.Entities;

namespace SchemaExtractor.Writers
{
    public class UserPrivilegeWriter
    {
        private readonly TextWriter _writer;

        public UserPrivilegeWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public void WriteSection(UserPrivilege[] privileges, string parentRelationName)
        {
            // NOTE: Assumes that SYSDBA is the grantor

            const string allText = "SELECT, INSERT, UPDATE, DELETE, REFERENCE";

            foreach (var userTypeGroup in privileges.GroupBy(a => a.RdbUserType))
            {
                _writer.WriteLine($"/* Privileges - {userTypeGroup.Key.FormatName()} */");

                foreach (var privilegeGroup in userTypeGroup.GroupBy(a => a.FormatUser()))
                {
                    var types = string.Join(", ", privilegeGroup.Select(a => a.FormatPrivilege()));

                   Write(privilegeGroup.First(), types == allText ? "ALL" : types);
                }

                _writer.WriteLine();
            }
        }

        private void Write(UserPrivilege privilege, string overrideType = null)
        {
            var typeText = overrideType ?? privilege.FormatPrivilege();
            var relationName = privilege.FormatRelationName();

            _writer.WriteLine($"GRANT {typeText} ON {relationName} TO {privilege.FormatUserObject()}");
        }
    }
}
