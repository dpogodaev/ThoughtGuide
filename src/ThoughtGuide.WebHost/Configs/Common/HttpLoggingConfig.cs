﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThoughtGuide.HostConfiguration.Extensions;
using ThoughtGuide.HostConfiguration.Interfaces;
using ThoughtGuide.WebHost.Settings;

namespace ThoughtGuide.WebHost.Configs.Common;

/// <summary>
/// HTTP logging configuration.
/// </summary>
internal static class HttpLoggingConfig
{
    private const string HttpLoggingConfigSection = "LoggingFeatures:HttpLogging";

    private const bool RequestPropertiesAndHeaders = false;
    private const bool RequestProtocol = true;
    private const bool RequestScheme = true;
    private const bool RequestMethod = true;
    private const bool RequestPath = true;
    private const bool RequestQuery = true;
    private const bool RequestBody = false;
    private const bool ResponsePropertiesAndHeaders = true;
    private const bool ResponseStatusCode = true;
    private const bool ResponseBody = false;

    /// <summary>
    /// Adds a configuration for HTTP logging.
    /// </summary>
    /// <remarks>
    /// Also need to add the use of the <see cref="HttpLoggingBuilderExtensions.UseHttpLogging"/> method.
    /// </remarks>
    public static void AddHttpLoggingConfig(this IServiceCollection services,
        IConfiguration configuration, IStartupLogger logger = null)
    {
        if (!configuration.SectionExists(HttpLoggingConfigSection))
        {
            logger?.Warn($"The configuration section '{HttpLoggingConfigSection}' is not specified");
            services.AddHttpLogging(opt => opt.LoggingFields = HttpLoggingFields.None); // This is required!
            return;
        }

        var settings = configuration.BindSection<HttpLoggingSettings>(HttpLoggingConfigSection);

        services.AddHttpLogging(opt => opt.LoggingFields =
            GetField(RequestPropertiesAndHeaders, settings.RequestHeaders,
                HttpLoggingFields.RequestPropertiesAndHeaders) |
            GetField(RequestProtocol, settings.RequestProtocol, HttpLoggingFields.RequestProtocol) |
            GetField(RequestScheme, settings.RequestScheme, HttpLoggingFields.RequestScheme) |
            GetField(RequestMethod, settings.RequestMethod, HttpLoggingFields.RequestMethod) |
            GetField(RequestPath, settings.RequestPath, HttpLoggingFields.RequestPath) |
            GetField(RequestQuery, settings.RequestQuery, HttpLoggingFields.RequestQuery) |
            GetField(RequestBody, settings.RequestBody, HttpLoggingFields.RequestBody) |
            GetField(ResponsePropertiesAndHeaders, settings.ResponseHeaders,
                HttpLoggingFields.ResponsePropertiesAndHeaders) |
            GetField(ResponseStatusCode, settings.ResponseStatusCode, HttpLoggingFields.ResponseStatusCode) |
            GetField(ResponseBody, settings.ResponseBody, HttpLoggingFields.ResponseBody)
        );
    }

    #region Private logic

    private static HttpLoggingFields GetField(bool defaultActivity, bool? isFieldActive, HttpLoggingFields field)
    {
        return isFieldActive ?? defaultActivity ? field : HttpLoggingFields.None;
    }

    #endregion
}