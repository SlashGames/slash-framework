<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="doc-def-template.xsl" />
  <xsl:output method="xml" encoding="UTF-8" omit-xml-declaration="yes" />
  <xsl:template match="Page">
    <html>
      <head>
        <title><xsl:value-of select="Title" /></title>
        <link rel="stylesheet" href="/style.css" type="text/css" />
        <meta http-equiv="Content-Type" content="text/html;charset=utf8" />
        <xsl:call-template name="create-default-style" />
        <xsl:call-template name="create-default-script" />
        <style>
          div.SideBar {
            width: 10em;
          }
          div.information {
            padding-left: 1em;
            padding-right: 1em;
            float: right;
            clear: both;
            width: 10em;
            border-color: black;
            border-style: solid;
            border-width: thin;
            background-color: #f2f2f2;
            margin-top: 2em;
          }
        </style>
        <script src="/doc/ndesk-options/prettyprint.js" type="text/javascript">
        // ignore
        </script>
      </head>
      <body>
        <!-- HEADER -->
        <xsl:call-template name="create-default-collection-title" />
        <xsl:call-template name="create-index" />
        <div class="information">
          <p><a href="/Options">NDesk.Options</a></p>
          <p><a href="http://git.ndesk.org/?p=ndesk-options;a=summary"
            >GIT Repository</a></p>
        </div>
        <xsl:call-template name="create-default-title" />
        <xsl:call-template name="create-default-summary" />
        <xsl:call-template name="create-default-signature" />
        <xsl:call-template name="create-default-remarks" />
        <xsl:call-template name="create-default-members" />
        <hr size="1" />
        <xsl:call-template name="create-default-copyright" />
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>

