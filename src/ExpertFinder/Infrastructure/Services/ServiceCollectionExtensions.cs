﻿using Azure;
using Azure.AI.OpenAI;
using ExpertFinder.Domain.Aggregates.ArticleAggregate;
using ExpertFinder.Domain.Services;
using ExpertFinder.Infrastructure.Persistence;
using ExpertFinder.Shared;
using ExpertFinder.Weaviate;
using Microsoft.Extensions.Options;

namespace ExpertFinder.Infrastructure.Services;

public static class ServiceCollectionExtensions
{
    public static void AddEmbeddingGenerator(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddScoped<OpenAIClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<EmbeddingGeneratorOptions>>();
            return new OpenAIClient(new Uri(options.Value.Endpoint), new AzureKeyCredential(options.Value.ApiKey));
        });

        services.AddScoped<IEmbeddingGenerator, EmbeddingGenerator>();

        services.Configure<EmbeddingGeneratorOptions>(configuration.GetSection("EmbeddingGenerator"));
    }

    public static void AddContentManager(this IServiceCollection services)
    {
        services.AddScoped<IArticleRepository, ArticleRepository>();
    }

    public static void AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddSearchEngine(this IServiceCollection services, Uri endpointUri)
    {
        services.AddWeaviateClient(endpointUri);
        services.AddScoped<ISearchEngine, SearchEngine>();
        services.AddWeaviateSearch().ConfigureHttpClient(client =>
        {
            client.BaseAddress = new($"{endpointUri}/v1/graphql");
        });
    }
}