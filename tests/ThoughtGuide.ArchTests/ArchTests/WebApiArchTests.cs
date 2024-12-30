﻿using System.Collections.Generic;
using ThoughtGuide.ArchTests.Constants;
using ThoughtGuide.ArchTests.Extensions;
using ThoughtGuide.ArchTests.Helpers;
using Xunit;

namespace ThoughtGuide.ArchTests.ArchTests;

/// <summary>
/// Architectural tests for <see cref="ThoughtGuide.WebApi"/> component.
/// </summary>
public class WebApiArchTests
{
    private const string WorkingNamespace = ComponentNamespaces.WebApi;

    /// <summary>
    /// Tests to check the dependency policy for <see cref="ThoughtGuide.WebApi"/> component.
    /// </summary>
    [Fact]
    public void DependencyOfComponentsShouldFollowCleanArchitecture()
    {
        //Arrange
        var policyDefinition = PolicyHelper.BuildPolicyDefinition(WorkingNamespace,
            "Components dependency policy",
            "Describes the dependencies of the 'WebApi' component");

        policyDefinition.Add(types => types
                .That().ResideInNamespace(WorkingNamespace)
                .ShouldNot().HaveDependenciesOtherThan(CreateAllowedDependenciesList([
                    ComponentNamespaces.Common,
                    ComponentNamespaces.WebAuth
                ])),
            "The rule of dependence of the 'WebApi' on other components",
            "The 'WebApi' component can only depend on the 'Common' component " +
            "and components that also implement the interface (e.g., 'WebAuth')");

        // Act
        var results = policyDefinition.Evaluate().Results;

        // Assert
        Assert.All(results, x => Assert.True(x.IsSuccessful));
    }

    /// <summary>
    /// Tests to check class naming of <see cref="ThoughtGuide.WebApi"/> component.
    /// </summary>
    [Fact]
    public void ClassNamesMustFollowNamingRules()
    {
        //Arrange
        var policyDefinition = PolicyHelper.BuildPolicyDefinition(WorkingNamespace,
            "File naming policy",
            "Describes the naming policy for files with the '.cs' extension");

        policyDefinition
            .AddAttributeNamingRule(WorkingNamespace)
            .AddControllerNamingRule(WorkingNamespace)
            .AddFilterNamingRule(WorkingNamespace)
            .AddModelNamingRule(WorkingNamespace);

        // Act
        var results = policyDefinition.Evaluate().Results;

        // Assert
        Assert.All(results, x => Assert.True(x.IsSuccessful));
    }

    #region Private logic

    private static string[] CreateAllowedDependenciesList(IEnumerable<string> allowedComponents)
    {
        var allowedDependenciesList = new List<string> { WorkingNamespace };
        allowedDependenciesList.AddRange(GetUsingLibs());
        allowedDependenciesList.AddRange(allowedComponents);

        return allowedDependenciesList.ToArray();
    }

    private static IEnumerable<string> GetUsingLibs()
    {
        return
        [
            "System",
            "Microsoft",
            "Swashbuckle"
        ];
    }

    #endregion
}