//using System;
//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.Diagnostics;
//using System.Diagnostics.CodeAnalysis;
//using System.Globalization;
//using System.Linq;
//using System.Reflection;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using ABI.Windows.ApplicationModel.Activation;
//using EasyEncounters.Attributes;
//using EasyEncounters.SourceGenerators.Extensions;
//using EasyEncounters.SourceGenerators.Helpers;
//using EasyEncounters.SourceGenerators.Model;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.UI.Xaml.Input;
//using Newtonsoft.Json.Linq;
//using Windows.System;
//using PropertyInfo = EasyEncounters.SourceGenerators.Model.PropertyInfo;

//namespace EasyEncounters.SourceGenerators;
//[Generator]
//public class ValidationGenerator : IIncrementalGenerator
//{
//    private const string ValidationAttributeName = nameof(ValidationAttribute);
//    //private const string 
//    //see: https://andrewlock.net/creating-a-source-generator-part-1-creating-an-incremental-source-generator/

//    //see also: https://stackoverflow.com/questions/70402988/how-to-completely-evaluate-an-attributes-parameters-in-a-c-sharp-source-generat
//    //see:https://youtu.be/iOp3mN933Og?t=2457
//    public void Initialize(IncrementalGeneratorInitializationContext context)
//    {
//        //syntactic: classes with greater than one attribute.
//        //semantic: classes with the correct attribute.
//        IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
//            .ForAttributeWithMetadataName("EasyEncounters.Attributes.ValidationAttrirbute",
//                static (node, _) => IsSyntaxTarget(node),
//                static (ctx, _) => GetSemanticTarget(ctx))
//            .Where(static m => m is not null);

//        //generate the code

//        //generate the source using the compilation
//    }





////take fields with tag. 

////IncrementalValuesProvider <ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
////            .ForAttributeWithMetadataName("EasyEncounters.Attributes.ValidationAttrirbute",
////            static (node, _) => 
////            node is VariableDeclarationSyntax { Parent: VariableDeclarationSyntax { Parent: FieldDeclarationSyntax { Parent: ClassDeclarationSyntax or RecordDeclarationSyntax, AttributeLists.Count: > 0 } } }, //cull to only possible fields
////            static (context, token) =>
////            {
////                FieldDeclarationSyntax fieldDeclaration = (FieldDeclarationSyntax)context.TargetNode.Parent!.Parent!;
////                IFieldSymbol fieldSymbol = (IFieldSymbol)context.TargetSymbol;

////                //with the field firmly selected, we now need to find info about its parents etc.
////                //so we need to iterate up to the highest level, collecting parent info at each step.
////                //easiest to use a version of the communitymvvmtoolkit's implementation rather than reinvent the wheel for this
////                HierarchyInfo hierarchy = HierarchyInfo.From(fieldSymbol.ContainingType);

////                token.ThrowIfCancellationRequested();

////            //now need to also find some property Info (not the reflection type) 
////            //todo:

                


////                token.ThrowIfCancellationRequested();

////            }
////            )

        

//        //IncrementalValuesProvider<(HierarchyInfo Heirarchy, PropertyInfo? Info)> propertyInfo =
//        //    context.SyntaxProvider
//        //    .ForAttributeWithMetadataName("EasyEncounters.Attributes.ValidationAttrirbute",
//        //    static (node, _) => node is VariableDeclarationSyntax { Parent: VariableDeclarationSyntax { Parent: FieldDeclarationSyntax { Parent: ClassDeclarationSyntax or RecordDeclarationSyntax, AttributeLists.Count: > 0 } } },
//        //    static (context, token) =>
//        //    {
//        //        FieldDeclarationSyntax fieldDeclaration = (FieldDeclarationSyntax)context.TargetNode.Parent!.Parent!;
//        //        IFieldSymbol fieldSymbol = (IFieldSymbol)context.TargetSymbol;

//        //        HierarchyInfo hierarchy = HierarchyInfo.From(fieldSymbol.ContainingType);

//        //        token.ThrowIfCancellationRequested();

//        //        _ = Execute.TryGetInfo(fieldDeclaration, fieldSymbol, context.SemanticModel, token, out PropertyInfo? propertyInfo);

//        //        token.ThrowIfCancellationRequested();

//        //        return (Hierarchy: hierarchy, propertyInfo);
//        //    }).Where(static item => item.Hierarchy is not null)
//        //    .Where(static item => item.propertyInfo is not null)!;

//        //    // Split and group by containing type
//        //    IncrementalValuesProvider<(HierarchyInfo Hierarchy, EquatableArray<PropertyInfo> Properties)> groupedPropertyInfo =
//        //        propertyInfo.GroupBy(static item => item.Left, static item => item.Right.Value);
    

//    private static bool TryGetInfo(
//        FieldDeclarationSyntax fieldSyntax,
//        IFieldSymbol fieldSymbol,
//        SemanticModel semanticModel,
//        CancellationToken token,
//        [NotNullWhen(true)] out PropertyInfo? propertyInfo)
//    {

//        string typeNameWithNullabilityuAnnotations = fieldSymbol.Type.GetFullyQualifiedNameWithNullabilityAnnotations();
//        string fieldName = fieldSymbol.Name;
//        string propertyName = GetGeneratedPropertyName(fieldSymbol);

//        if (fieldName == propertyName) //ignore capitalized properties
//        {
//            propertyInfo = null;
//            return false;
//        }

//        token.ThrowIfCancellationRequested();

//        if(IsGeneratedPropertyInvalid(propertyName, fieldSymbol.Type))
//        {
//            propertyInfo = null;
//            return false;
//        }

//        token.ThrowIfCancellationRequested();




//    }

//    /// <summary>
//    /// Checks whether the generated property would be a special case that is marked as invalid.
//    /// </summary>
//    /// <param name="propertyName">The property name.</param>
//    /// <param name="propertyType">The property type.</param>
//    /// <returns>Whether the generated property is invalid.</returns>
//    private static bool IsGeneratedPropertyInvalid(string propertyName, ITypeSymbol propertyType)
//    {
//        // If the generated property name is called "Property" and the type is either object or it is PropertyChangedEventArgs or
//        // PropertyChangingEventArgs (or a type derived from either of those two types), consider it invalid. This is needed because
//        // if such a property was generated, the partial On<PROPERTY_NAME>Changing and OnPropertyChanging(PropertyChangingEventArgs)
//        // methods, as well as the partial On<PROPERTY_NAME>Changed and OnPropertyChanged(PropertyChangedEventArgs) methods.
//        if (propertyName == "Property")
//        {
//            return
//                propertyType.SpecialType == SpecialType.System_Object ||
//                propertyType.HasOrInheritsFromFullyQualifiedMetadataName("System.ComponentModel.PropertyChangedEventArgs") ||
//                propertyType.HasOrInheritsFromFullyQualifiedMetadataName("System.ComponentModel.PropertyChangingEventArgs");
//        }

//        return false;
//    }

//    /// <summary>
//    /// Generates a property name for a given fieldSymbol, capitalizing the first letter and removing m_ or _ field prefixes.
//    /// </summary>
//    /// <param name="fieldSymbol"></param>
//    /// <returns></returns>
//    private static string GetGeneratedPropertyName(IFieldSymbol fieldSymbol)
//    {
//        string propertyName = fieldSymbol.Name;
//        if (propertyName.StartsWith("m_"))
//            propertyName = propertyName.Substring(2);
//        else if (propertyName.StartsWith("_"))
//            propertyName = propertyName.TrimStart('_');

//        return $"{char.ToUpper(propertyName[0], CultureInfo.InvariantCulture)}{propertyName.Substring(1)}";
//    }

//    //good practice with generated code:
//    // //<auto-generated/> <- suppresses errors afaik
//    // #nullable enable    < - enable nullability, duh
//    //also good practice to add GeneratedCodeAttribute, but eh

//    //'build' to classname.g.cs <- indicates generated code. 

//}
