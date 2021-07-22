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
        <TargetFramework>net5.0</TargetFramework>
    </PropertyGroup>

    <ItemGroup>
      <!-- These are hard coded; maybe they shouldn't be. -->
        <EmbeddedResource Include="Data/catalog.xml">
                <Link>Org.XmlResolver.catalog.xml</Link>
        </EmbeddedResource>
        <EmbeddedResource Include="Data/cat-entities.xml">
                <Link>Org.XmlResolver.cat-entities.xml</Link>
        </EmbeddedResource>
        <EmbeddedResource Include="Data/cat-mathml2.xml">
                <Link>Org.XmlResolver.cat-mathml2.xml</Link>
        </EmbeddedResource>
        <EmbeddedResource Include="Data/cat-mathml3.xml">
                <Link>Org.XmlResolver.cat-mathml3.xml</Link>
        </EmbeddedResource>
        <EmbeddedResource Include="Data/cat-rddl.xml">
                <Link>Org.XmlResolver.cat-rddl.xml</Link>
        </EmbeddedResource>
        <EmbeddedResource Include="Data/cat-svg.xml">
                <Link>Org.XmlResolver.cat-svg.xml</Link>
        </EmbeddedResource>
        <EmbeddedResource Include="Data/cat-xhtml.xml">
                <Link>Org.XmlResolver.cat-xhtml.xml</Link>
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

<xsl:template match="system|uri">
  <EmbeddedResource Include="Data/{substring-after(../@uri, 'root/')}">
    <Link>
    <xsl:sequence select="'Org.XmlResolver.' || f:patch-uri(substring-after(../@uri, 'root/'))"/>
    </Link>
  </EmbeddedResource>
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
