<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns="urn:oasis:names:tc:entity:xmlns:xml:catalog"
                xmlns:f="https://xmlresolver.org/ns/xslt/functions"
                xmlns:xs="http://www.w3.org/2001/XMLSchema"
                xmlns:map="http://www.w3.org/2005/xpath-functions/map"
                exclude-result-prefixes="f map xs"
                version="3.0">

<xsl:output method="xml" encoding="utf-8" indent="yes"/>
<xsl:strip-space elements="*"/>

<xsl:param name="generate-tests" select="()"/>

<xsl:template match="/">
  <xsl:variable name="entries" as="element()*">
    <xsl:apply-templates select="//public|//system|//uri"
                         mode="catalog-entry"/>
  </xsl:variable>
  <xsl:variable name="subset" select="()"/>
  <xsl:variable name="prefix" select="()"/>

  <!-- ============================================================ -->

  <xsl:if test="exists($generate-tests)">
    <xsl:result-document href="{$generate-tests}" method="text">
      <xsl:apply-templates select="//public|//system|//uri" mode="generate-tests"/>
    </xsl:result-document>
  </xsl:if>

  <!-- ============================================================ -->

  <xsl:variable name="entries" select="$entries except $subset"/>

  <xsl:variable name="prefix" as="node()*">
    <xsl:sequence select="$prefix"/>
    <delegatePublic publicIdStartString="-//W3C//ENTITIES XHTML"
                    catalog="Org.XmlResolver.cat-xhtml.xml"/>
    <delegatePublic publicIdStartString="-//W3C//ELEMENTS XHTML"
                    catalog="Org.XmlResolver.cat-xhtml.xml"/>
    <delegatePublic publicIdStartString="-//W3C//DTD XHTML"
                    catalog="Org.XmlResolver.cat-xhtml.xml"/>
    <delegateSystem systemIdStartString="https://www.w3.org/MarkUp/DTD/xhtml"
                    catalog="Org.XmlResolver.cat-xhtml.xml"/>
    <delegateSystem systemIdStartString="https://www.w3.org/TR/xhtml"
                    catalog="Org.XmlResolver.cat-xhtml.xml"/>
  </xsl:variable>

  <xsl:variable name="subset"
                select="$entries[starts-with(@publicId, '-//W3C//ENTITIES XHTML')
                                 or starts-with(@publicId, '-//W3C//ELEMENTS XHTML')
                                 or starts-with(@systemId,
                                       'https://www.w3.org/MarkUp/DTD/xhtml')
                                 or starts-with(@systemId,
                                       'https://www.w3.org/TR/xhtml')
                                 or starts-with(@publicId, '-//W3C//DTD XHTML')
                                ]"/>

  <xsl:result-document href="cat-xhtml.xml">
    <catalog prefer="public">
      <xsl:sequence select="$subset"/>
    </catalog>
  </xsl:result-document>

  <!-- ============================================================ -->

  <xsl:variable name="entries" select="$entries except $subset"/>

  <xsl:variable name="prefix" as="node()*">
    <xsl:sequence select="$prefix"/>
    <delegatePublic publicIdStartString="-//W3C//ENTITIES SVG"
                    catalog="Org.XmlResolver.cat-svg.xml"/>
    <delegatePublic publicIdStartString="-//W3C//ELEMENTS SVG"
                    catalog="Org.XmlResolver.cat-svg.xml"/>
    <delegatePublic publicIdStartString="-//W3C//DTD SVG"
                    catalog="Org.XmlResolver.cat-svg.xml"/>
    <delegateSystem systemIdStartString="https://www.w3.org/Graphics/SVG/1.1/"
                    catalog="Org.XmlResolver.cat-svg.xml"/>
  </xsl:variable>

  <xsl:variable name="subset"
                select="$entries[starts-with(@publicId, '-//W3C//ENTITIES SVG')
                                 or starts-with(@publicId, '-//W3C//ELEMENTS SVG ')
                                 or starts-with(@systemId,
                                      'https://www.w3.org/Graphics/SVG/1.1/')
                                 or starts-with(@publicId, '-//W3C//DTD SVG ')
                                ]"/>

  <xsl:result-document href="cat-svg.xml">
    <catalog prefer="public">
      <xsl:sequence select="$subset"/>
    </catalog>
  </xsl:result-document>

  <!-- ============================================================ -->

  <xsl:variable name="entries" select="$entries except $subset"/>

  <!-- These don't have a useful form for delegation, so leave them in misc -->
  <xsl:variable name="mathml-public"
                select="$entries[(starts-with(@publicId, '-//W3C//ENTITIES ')
                                  and contains(@publicId, 'MathML 2.0'))]"/>

  <xsl:variable name="entries" select="$entries except $mathml-public"/>

  <xsl:variable name="prefix" as="node()*">
    <xsl:sequence select="$prefix"/>
    <delegatePublic publicIdStartString="-//W3C//ENTITIES "
                    catalog="Org.XmlResolver.cat-entities.xml"/>
    <delegatePublic publicIdStartString="ISO 8879:1986//ENTITIES "
                    catalog="Org.XmlResolver.cat-entities.xml"/>
    <delegateSystem systemIdStartString="https://www.w3.org/2003/entities/"
                    catalog="Org.XmlResolver.cat-entities.xml"/>
    <delegateSystem systemIdStartString="https://www.w3.org/Math/DTD/mathml1/"
                    catalog="Org.XmlResolver.cat-entities.xml"/>
  </xsl:variable>

  <xsl:variable name="subset"
                select="$entries[starts-with(@publicId, '-//W3C//ENTITIES ')
                                 or starts-with(@publicId, 'ISO 8879:1986//ENTITIES ')
                                 or starts-with(@systemId,
                                      'https://www.w3.org/2003/entities/')
                                 or starts-with(@systemId,
                                      'https://www.w3.org/Math/DTD/mathml1/')
                                ]"/>

  <xsl:result-document href="cat-entities.xml">
    <catalog prefer="public">
      <xsl:sequence select="$subset"/>
    </catalog>
  </xsl:result-document>

  <!-- ============================================================ -->

  <xsl:variable name="entries" select="$entries except $subset"/>

  <xsl:variable name="prefix" as="node()*">
    <xsl:sequence select="$prefix"/>
    <delegateSystem systemIdStartString="https://www.w3.org/Math/DTD/mathml2/"
                    catalog="Org.XmlResolver.cat-mathml2.xml"/>
  </xsl:variable>

  <xsl:variable name="subset"
                select="$entries[starts-with(@systemId,
                                   'https://www.w3.org/Math/DTD/mathml2/')]"/>
  <xsl:result-document href="cat-mathml2.xml">
    <catalog prefer="public">
      <xsl:sequence select="$subset"/>
    </catalog>
  </xsl:result-document>

  <!-- ============================================================ -->

  <xsl:variable name="entries" select="$entries except $subset"/>

  <xsl:variable name="prefix" as="node()*">
    <xsl:sequence select="$prefix"/>
    <delegateSystem systemIdStartString="https://www.w3.org/Math/DTD/mathml3/"
                    catalog="Org.XmlResolver.cat-mathml3.xml"/>
  </xsl:variable>

  <xsl:variable name="subset"
                select="$entries[starts-with(@systemId,
                                   'https://www.w3.org/Math/DTD/mathml3/')]"/>
  <xsl:result-document href="cat-mathml3.xml">
    <catalog prefer="public">
      <xsl:sequence select="$subset"/>
    </catalog>
  </xsl:result-document>

  <!-- ============================================================ -->

  <xsl:variable name="entries" select="$entries except $subset"/>

  <xsl:variable name="prefix" as="node()*">
    <xsl:sequence select="$prefix"/>
    <xsl:text>&#10;</xsl:text>
    <xsl:comment> At present, only the RDDL public identifiers begin with -//XML-DEV// </xsl:comment>
    <delegatePublic publicIdStartString="-//XML-DEV//"
                    catalog="Org.XmlResolver.cat-rddl.xml"/>
    <delegateSystem systemIdStartString="http://www.rddl.org/"
                    catalog="Org.XmlResolver.cat-rddl.xml"/>
  </xsl:variable>

  <xsl:variable name="subset"
                select="$entries[starts-with(@publicId, '-//XML-DEV//')
                                 or starts-with(@systemId, 'http://www.rddl')]"/>

  <xsl:result-document href="cat-rddl.xml">
    <catalog prefer="public">
      <xsl:sequence select="$subset"/>
    </catalog>
  </xsl:result-document>

  <!-- ============================================================ -->

  <xsl:variable name="entries" select="$entries except $subset"/>

  <xsl:result-document href="catalog.xml">
    <catalog prefer="public">
      <xsl:sequence select="$prefix"/>
      <xsl:sequence select="$entries"/>
      <xsl:comment> The MathML public identifiers below aren’t in one of the  </xsl:comment>
      <xsl:comment> delegated catalogs because the form of the identifier     </xsl:comment>
      <xsl:comment> doesn’t lend itself to delegation. Simplest to just leave </xsl:comment>
      <xsl:comment> them here. </xsl:comment>
      <xsl:sequence select="$mathml-public"/>
    </catalog>
  </xsl:result-document>
</xsl:template>

<xsl:template match="manifest">
  <catalog prefer="public">
    <xsl:apply-templates/>
  </catalog>
</xsl:template>

<xsl:template match="element()">
  <xsl:message>Unexpected element: <xsl:sequence select="node-name(.)"/></xsl:message>
</xsl:template>

<xsl:template match="attribute()|text()|comment()|processing-instruction()">
  <xsl:copy/>
</xsl:template>

<!-- ============================================================ -->

<xsl:template match="public" mode="catalog-entry">
  <public publicId="{.}" uri="{f:patch-uri(substring-after(../@uri, 'root/'))}"/>
</xsl:template>

<xsl:template match="system" mode="catalog-entry">
  <system systemId="{.}" uri="{f:patch-uri(substring-after(../@uri, 'root/'))}"/>
</xsl:template>

<xsl:template match="uri" mode="catalog-entry">
  <uri name="{.}" uri="{f:patch-uri(substring-after(../@uri, 'root/'))}"/>
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

<!-- ============================================================ -->

<xsl:template match="public" mode="generate-tests">
  <xsl:text>    [Test]&#10;</xsl:text>
  <xsl:text>    public void Gen_lookupPublic</xsl:text>
  <xsl:sequence select="generate-id(.)"/>
  <xsl:text>() {&#10;</xsl:text>
  <xsl:text>        Uri result = manager.LookupPublic(null, "</xsl:text>
  <xsl:value-of select="."/>
  <xsl:text>");&#10;</xsl:text>
  <xsl:text>        Assert.NotNull(result);&#10;</xsl:text>
  <xsl:text>    }&#10;&#10;</xsl:text>
</xsl:template>

<xsl:template match="system" mode="generate-tests">
  <xsl:text>    [Test]&#10;</xsl:text>
  <xsl:text>    public void Gen_lookupSystem</xsl:text>
  <xsl:sequence select="generate-id(.)"/>
  <xsl:text>() {&#10;</xsl:text>
  <xsl:text>        Uri result = manager.LookupSystem("</xsl:text>
  <xsl:value-of select="."/>
  <xsl:text>");&#10;</xsl:text>
  <xsl:text>        Assert.NotNull(result);&#10;</xsl:text>
  <xsl:text>    }&#10;&#10;</xsl:text>
</xsl:template>

<xsl:template match="uri" mode="generate-tests">
  <xsl:text>    [Test]&#10;</xsl:text>
  <xsl:text>    public void Gen_lookupUri</xsl:text>
  <xsl:sequence select="generate-id(.)"/>
  <xsl:text>() {&#10;</xsl:text>
  <xsl:text>        Uri result = manager.LookupUri("</xsl:text>
  <xsl:value-of select="."/>
  <xsl:text>");&#10;</xsl:text>
  <xsl:text>        Assert.NotNull(result);&#10;</xsl:text>
  <xsl:text>    }&#10;&#10;</xsl:text>
</xsl:template>

</xsl:stylesheet>
