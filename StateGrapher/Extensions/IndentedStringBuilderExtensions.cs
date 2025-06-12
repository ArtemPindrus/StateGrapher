using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateGrapher.Extensions {
    public static class IndentedStringBuilderExtensions {
        public static IndentedStringBuilder AppendBlock(this IndentedStringBuilder sb, string prefix, string block) {
            sb.AppendLine($"{prefix} {{")
                .IncrementIndent()
                .AppendLines(block)
                .DecrementIndent()
                .AppendLine("}");

            return sb;
        }

        public static IndentedStringBuilder AppendBlock(this IndentedStringBuilder sb, string prefix, Action<IndentedStringBuilder> buildAction, string? postfix = null) {
            sb.AppendLine($"{prefix} {{")
                .IncrementIndent();

            buildAction.Invoke(sb);
                
            sb.DecrementIndent()
                .Append($"}}{postfix}")
                .AppendLine();

            return sb;
        }
    }
}
