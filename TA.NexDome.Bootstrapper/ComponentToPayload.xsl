<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:wix="http://schemas.microsoft.com/wix/2006/wi"
  xmlns="http://schemas.microsoft.com/wix/2006/wi">

  <xsl:template match="/">
    <Wix>
      <Fragment>
        <xsl:apply-templates select="*" />
      </Fragment>
    </Wix>
  </xsl:template>

  <xsl:template match="//wix:ComponentGroup">
    <PayloadGroup>
      <xsl:attribute name="Id">
        <xsl:value-of select="@Id"/>
      </xsl:attribute>
      <xsl:apply-templates select="*" />
    </PayloadGroup>
  </xsl:template>

  <xsl:template match="//wix:File">
    <Payload>
      <xsl:attribute name="SourceFile">
        <xsl:value-of select="@Source"/>
      </xsl:attribute>
    </Payload>
  </xsl:template>

</xsl:stylesheet>