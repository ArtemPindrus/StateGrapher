using Microsoft.EntityFrameworkCore.Infrastructure;

namespace StateGrapher.Utilities
{
    public static class ClassBuilder {
        public static IndentedStringBuilder AppendEnum(this IndentedStringBuilder sb, string prefix, string enumName, IEnumerable<string> members) {
            sb.AppendLine($"{prefix} enum {enumName} {{")
                .IncrementIndent();

            foreach (var m in members) {
                sb.AppendLine($"{m},");
            }

            sb.DecrementIndent()
                .AppendLine("}");

            return sb;
        }
    }
}
