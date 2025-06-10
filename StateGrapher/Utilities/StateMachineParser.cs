using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StateGrapher.Extensions;
using StateGrapher.Models;
using System.Buffers;
using System.Printing;
using System.Text.RegularExpressions;
using System.Windows.Automation.Peers;
using System.Xml.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Nodify.Interactivity.MultiGesture;

namespace StateGrapher.Utilities
{
    public static class StateMachineParser
    {
        const string voidIdentifier = "void";

        /// <summary>
        /// Generates C# state-machine class based on <see cref="StateMachine"/> instance.
        /// </summary>
        /// <param name="rootSm"></param>
        /// <returns></returns>
        public static string GenerateCSharpClass(StateMachine rootSm) {
            List<GenerationError> errors = new();
            string name = rootSm.Name ?? "StateMachine";

            var allNodes = rootSm.GetHierarchyNodes(true).ToArray();
            var allConnections = rootSm.GetHierarchyConnections().ToArray();

            var eventIdEnum = CreateEventIdEnum(allConnections);

            var stateId = EnumDeclaration("StateId")
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .AddMembers(
                    allNodes
                        .Select(x => EnumMemberDeclaration(x.Name))
                        .ToArray()
                );

            var stateIdField = FieldDeclaration(
                VariableDeclaration(
                    IdentifierName(stateId.Identifier)
                ).WithVariables(
                    SingletonSeparatedList(
                        VariableDeclarator("stateId")
                    )    
                )
            ).AddModifiers(Token(SyntaxKind.PublicKeyword));

            List<MethodDeclarationSyntax> methods = new();
            foreach (var node in allNodes) {
                GenerateEventHandlers(node, allConnections, methods);
            }

            ClassDeclarationSyntax classDeclarationSyntax = ClassDeclaration(name)
                .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.PartialKeyword))
                .AddMembers(eventIdEnum, stateId, stateIdField)
                .AddMembers(methods.ToArray());

            string classString = classDeclarationSyntax
                .NormalizeWhitespace()
                .ToFullString();


            return classString;
        }

        private static void GenerateEventHandlers(StateMachine sm, IEnumerable<Connection> connections, List<MethodDeclarationSyntax> list) {
            var preHandlersComment = TriviaList(
                Comment("/////"),
                LineFeed,
                Comment("// Event handlers for state ROOT"),
                LineFeed,
                Comment("/////\n")
            );

            if (sm.Name == "ROOT") {
                var method = MethodDeclaration(IdentifierName(voidIdentifier), "ROOT_enter")
                    .AddBodyStatements(
                        ExpressionStatement(
                            AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                IdentifierName("stateId"),
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("StateId"),
                                    IdentifierName("ROOT")
                                )
                            )
                        )
                    ).WithLeadingTrivia(preHandlersComment);

                list.Add(method);
            }


        }

        private static EnumDeclarationSyntax CreateEventIdEnum(IEnumerable<Connection> connections) {
            var validConnections = connections
                .Where(x => x.Name != null)
                .DistinctBy(x => x.Name)
                .Where(x => !x.Name!.Contains("via"))
                .ToArray();

            var enumMembers = new List<EnumMemberDeclarationSyntax>();

            foreach (var connection in validConnections) { 
                if (connection.IsBothWays) {
                    enumMembers.Add(EnumMemberDeclaration(connection.BackEvent));
                }

                enumMembers.Add(EnumMemberDeclaration(connection.ForwardEvent));
            }

            var eventIdEnum = EnumDeclaration("EventId")
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .AddMembers(enumMembers.ToArray());

            return eventIdEnum;
        }
    }

    public class GenerationError {

    }
}
