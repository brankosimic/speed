﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0612, CS0618

internal partial class AppJsonContext
{
    private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<string>? _String;
    
    /// <summary>
    /// Defines the source generated JSON serialization contract metadata for a given type.
    /// </summary>
    #nullable disable annotations // Marking the property type as nullable-oblivious.
    public global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<string> String
    #nullable enable annotations
    {
        get => _String ??= (global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<string>)Options.GetTypeInfo(typeof(string));
    }
    
    private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<string> Create_String(global::System.Text.Json.JsonSerializerOptions options)
    {
        if (!TryGetTypeInfoForRuntimeCustomConverter<string>(options, out global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<string> jsonTypeInfo))
        {
            jsonTypeInfo = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreateValueInfo<string>(options, global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.StringConverter);
        }
    
        jsonTypeInfo.OriginatingResolver = this;
        return jsonTypeInfo;
    }
}
