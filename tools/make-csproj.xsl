<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:f="https://xmlresolver.org/ns/xslt/functions"
                xmlns:xs="http://www.w3.org/2001/XMLSchema"
                xmlns:map="http://www.w3.org/2005/xpath-functions/map"
                exclude-result-prefixes="f map xs"
                version="3.0">

<xsl:output method="xml" encoding="utf-8" indent="yes"/>
<xsl:strip-space elements="*"/>

<xsl:template match="/">
  <Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
      <TargetFramework>netstandard2.0</TargetFramework>
      <GeneratePackageOnBuild>True</GeneratePackageOnBuild>
      <Authors>Norman Walsh</Authors>
      <Description>This package provides a common set of XML resources and an XML Catalog that resolves them. It’s most commonly used with the xmlresolver package.</Description>
      <PackageTags>XML,XmlResolver</PackageTags>
      <PackageLicenseExpression>MIT</PackageLicenseExpression>
      <PackageProjectUrl>https://xmlresolver.org/</PackageProjectUrl>
      <RepositoryUrl>https://github.com/xmlresolver/xmlresolverdata</RepositoryUrl>
      <RepositoryType>git</RepositoryType>
      <PackageReleaseNotes>Standard resources for the catalog-based XML Resolver API for .NET</PackageReleaseNotes>
      <PackageSummary>This is a standard set of useful XML resources for use with the XML Resolver.</PackageSummary>
    </PropertyGroup>

    <ItemGroup>
      <!-- These are hard coded; maybe they shouldn't be. -->
        <EmbeddedResource Include="Data/catalog.xml">
                <LogicalName>XmlResolver.catalog.xml</LogicalName>
        </EmbeddedResource>
        <EmbeddedResource Include="Data/cat-entities.xml">
                <LogicalName>XmlResolver.cat-entities.xml</LogicalName>
        </EmbeddedResource>
        <EmbeddedResource Include="Data/cat-mathml2.xml">
                <LogicalName>XmlResolver.cat-mathml2.xml</LogicalName>
        </EmbeddedResource>
        <EmbeddedResource Include="Data/cat-mathml3.xml">
                <LogicalName>XmlResolver.cat-mathml3.xml</LogicalName>
        </EmbeddedResource>
        <EmbeddedResource Include="Data/cat-rddl.xml">
                <LogicalName>XmlResolver.cat-rddl.xml</LogicalName>
        </EmbeddedResource>
        <EmbeddedResource Include="Data/cat-svg.xml">
                <LogicalName>XmlResolver.cat-svg.xml</LogicalName>
        </EmbeddedResource>
        <EmbeddedResource Include="Data/cat-xhtml.xml">
                <LogicalName>XmlResolver.cat-xhtml.xml</LogicalName>
        </EmbeddedResource>
        <xsl:apply-templates select="//system|//uri"/>
    </ItemGroup>
  </Project>
</xsl:template>

<xsl:template match="element()">
  <xsl:message>Unexpected element: <xsl:sequence select="node-name(.)"/></xsl:message>
</xsl:template>

<xsl:template match="attribute()|text()|comment()|processing-instruction()">
  <xsl:copy/>
</xsl:template>

<!-- ============================================================ -->

<xsl:template match="system">
  <xsl:choose>
    <xsl:when test="preceding-sibling::system">
      <!-- skip dupliates in the .csproj file mapping -->
    </xsl:when>
    <xsl:otherwise>
      <EmbeddedResource Include="Data/{substring-after(../@uri, 'root/')}">
        <LogicalName>
          <xsl:sequence select="f:patch-uri(substring-after(../@uri, 'root/'))"/>
        </LogicalName>
      </EmbeddedResource>
    </xsl:otherwise>
  </xsl:choose>
</xsl:template>

<xsl:template match="uri">
  <xsl:choose>
    <xsl:when test="preceding-sibling::uri">
      <!-- skip dupliates in the .csproj file mapping -->
    </xsl:when>
    <xsl:otherwise>
      <EmbeddedResource Include="Data/{substring-after(../@uri, 'root/')}">
        <LogicalName>
          <xsl:sequence select="f:patch-uri(substring-after(../@uri, 'root/'))"/>
        </LogicalName>
      </EmbeddedResource>
    </xsl:otherwise>
  </xsl:choose>
</xsl:template>

<xsl:function name="f:patch-uri" as="xs:string">
  <xsl:param name="uri" as="xs:string"/>
  <!-- Transform the URI into a string that can be used as an
       EmbeddedResource Link -->
  <xsl:choose>
    <xsl:when test="contains($uri, '/')">
      <xsl:variable name="parts" select="tokenize($uri, '/')"/>
      <xsl:variable name="path" select="string-join($parts[position() lt last()], '/') || '/'"/>
      <xsl:variable name="filename" select="$parts[last()]"/>
      <xsl:sequence select="(replace($path, '\.', '_') => replace('/', '.')) || $filename"/>
    </xsl:when>
    <xsl:otherwise>
      <xsl:sequence select="$uri"/>
    </xsl:otherwise>
  </xsl:choose>
</xsl:function>

</xsl:stylesheet>
