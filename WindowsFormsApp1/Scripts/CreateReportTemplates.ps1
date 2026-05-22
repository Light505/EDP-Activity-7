Add-Type -AssemblyName WindowsBase

$root = Split-Path -Parent $PSScriptRoot
$logoPath = Join-Path $root 'Resources\company-logo.png'
$templateDir = Join-Path $root 'ReportTemplates'

if (-not (Test-Path $templateDir)) {
    New-Item -ItemType Directory -Path $templateDir | Out-Null
}

function Write-Part {
    param(
        [System.IO.Packaging.Package]$Package,
        [string]$Path,
        [string]$ContentType,
        [string]$Xml
    )

    $uri = [Uri]::new($Path, [UriKind]::Relative)
    if ($Package.PartExists($uri)) {
        $Package.DeletePart($uri)
    }

    $part = $Package.CreatePart($uri, $ContentType, [System.IO.Packaging.CompressionOption]::Maximum)
    $writer = [System.IO.StreamWriter]::new($part.GetStream([System.IO.FileMode]::Create, [System.IO.FileAccess]::Write), [System.Text.UTF8Encoding]::new($false))
    try {
        $writer.Write($Xml)
    }
    finally {
        $writer.Dispose()
    }

    return $part
}

function Add-BinaryPart {
    param(
        [System.IO.Packaging.Package]$Package,
        [string]$Path,
        [string]$ContentType,
        [string]$Source
    )

    $uri = [Uri]::new($Path, [UriKind]::Relative)
    if ($Package.PartExists($uri)) {
        $Package.DeletePart($uri)
    }

    $part = $Package.CreatePart($uri, $ContentType, [System.IO.Packaging.CompressionOption]::Maximum)
    $sourceStream = [System.IO.File]::OpenRead($Source)
    $targetStream = $part.GetStream([System.IO.FileMode]::Create, [System.IO.FileAccess]::Write)
    try {
        $sourceStream.CopyTo($targetStream)
    }
    finally {
        $targetStream.Dispose()
        $sourceStream.Dispose()
    }

    return $part
}

function New-WorkbookXml {
    param([string]$ReportTitle)

    return @"
<?xml version="1.0" encoding="utf-8"?>
<workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
  <sheets>
    <sheet name="$ReportTitle" sheetId="1" r:id="rId1" />
    <sheet name="Graph" sheetId="2" r:id="rId2" />
  </sheets>
</workbook>
"@
}

function New-StylesXml {
    return @'
<?xml version="1.0" encoding="utf-8"?>
<styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
  <fonts count="3">
    <font><sz val="11" /><name val="Calibri" /></font>
    <font><b /><sz val="11" /><name val="Calibri" /></font>
    <font><b /><sz val="16" /><name val="Calibri" /></font>
  </fonts>
  <fills count="3">
    <fill><patternFill patternType="none" /></fill>
    <fill><patternFill patternType="gray125" /></fill>
    <fill><patternFill patternType="solid"><fgColor rgb="FFD9EAF7" /><bgColor indexed="64" /></patternFill></fill>
  </fills>
  <borders count="2">
    <border><left /><right /><top /><bottom /><diagonal /></border>
    <border><left style="thin" /><right style="thin" /><top style="thin" /><bottom style="thin" /><diagonal /></border>
  </borders>
  <cellStyleXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0" /></cellStyleXfs>
  <cellXfs count="5">
    <xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0" />
    <xf numFmtId="0" fontId="1" fillId="0" borderId="0" xfId="0" applyFont="1" />
    <xf numFmtId="0" fontId="2" fillId="0" borderId="0" xfId="0" applyFont="1" />
    <xf numFmtId="0" fontId="1" fillId="2" borderId="1" xfId="0" applyFont="1" applyFill="1" applyBorder="1" />
    <xf numFmtId="0" fontId="0" fillId="0" borderId="1" xfId="0" applyBorder="1" />
  </cellXfs>
  <cellStyles count="1"><cellStyle name="Normal" xfId="0" builtinId="0" /></cellStyles>
</styleSheet>
'@
}

function New-Sheet1Xml {
    param([string]$ReportTitle)

    return @"
<?xml version="1.0" encoding="utf-8"?>
<worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
  <dimension ref="A1:H9" />
  <sheetFormatPr defaultRowHeight="15" />
  <cols>
    <col min="1" max="8" width="18" customWidth="1" />
  </cols>
  <sheetData>
    <row r="1"><c r="A1" s="2" t="inlineStr"><is><t>Academic Information System</t></is></c></row>
    <row r="2"><c r="A2" s="1" t="inlineStr"><is><t>$ReportTitle</t></is></c></row>
    <row r="4"><c r="A4" t="inlineStr"><is><t>Prepared by:</t></is></c></row>
    <row r="5"><c r="A5" t="inlineStr"><is><t>_____________________________</t></is></c></row>
    <row r="6"><c r="A6" t="inlineStr"><is><t>Signature</t></is></c></row>
    <row r="8"><c r="A8" s="3" t="inlineStr"><is><t>Column 1</t></is></c><c r="B8" s="3" t="inlineStr"><is><t>Column 2</t></is></c></row>
    <row r="9"><c r="A9" s="4" t="inlineStr"><is><t>Data</t></is></c><c r="B9" s="4" t="inlineStr"><is><t>Data</t></is></c></row>
  </sheetData>
  <mergeCells count="2">
    <mergeCell ref="A1:H1" />
    <mergeCell ref="A2:H2" />
  </mergeCells>
  <drawing r:id="rId1" />
</worksheet>
"@
}

function New-Sheet2Xml {
    param([string]$ReportTitle)

    return @"
<?xml version="1.0" encoding="utf-8"?>
<worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
  <dimension ref="A1:H22" />
  <sheetFormatPr defaultRowHeight="15" />
  <cols>
    <col min="1" max="2" width="20" customWidth="1" />
    <col min="4" max="8" width="14" customWidth="1" />
  </cols>
  <sheetData>
    <row r="1"><c r="A1" s="2" t="inlineStr"><is><t>$ReportTitle Graph</t></is></c></row>
    <row r="3"><c r="A3" s="3" t="inlineStr"><is><t>Category</t></is></c><c r="B3" s="3" t="inlineStr"><is><t>Record Count</t></is></c></row>
    <row r="4"><c r="A4" s="4" t="inlineStr"><is><t>Sample</t></is></c><c r="B4" s="4"><v>1</v></c></row>
  </sheetData>
  <mergeCells count="1">
    <mergeCell ref="A1:H1" />
  </mergeCells>
  <drawing r:id="rId1" />
</worksheet>
"@
}

function New-LogoDrawingXml {
    return @'
<?xml version="1.0" encoding="utf-8"?>
<xdr:wsDr xmlns:xdr="http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" xmlns:a="http://schemas.openxmlformats.org/drawingml/2006/main">
  <xdr:twoCellAnchor editAs="oneCell">
    <xdr:from><xdr:col>6</xdr:col><xdr:colOff>0</xdr:colOff><xdr:row>0</xdr:row><xdr:rowOff>0</xdr:rowOff></xdr:from>
    <xdr:to><xdr:col>7</xdr:col><xdr:colOff>609600</xdr:colOff><xdr:row>4</xdr:row><xdr:rowOff>0</xdr:rowOff></xdr:to>
    <xdr:pic>
      <xdr:nvPicPr><xdr:cNvPr id="2" name="Company Logo" /><xdr:cNvPicPr /></xdr:nvPicPr>
      <xdr:blipFill><a:blip xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships" r:embed="rId1" /><a:stretch><a:fillRect /></a:stretch></xdr:blipFill>
      <xdr:spPr><a:prstGeom prst="rect"><a:avLst /></a:prstGeom></xdr:spPr>
    </xdr:pic>
    <xdr:clientData />
  </xdr:twoCellAnchor>
</xdr:wsDr>
'@
}

function New-ChartDrawingXml {
    return @'
<?xml version="1.0" encoding="utf-8"?>
<xdr:wsDr xmlns:xdr="http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" xmlns:a="http://schemas.openxmlformats.org/drawingml/2006/main">
  <xdr:twoCellAnchor>
    <xdr:from><xdr:col>3</xdr:col><xdr:colOff>0</xdr:colOff><xdr:row>2</xdr:row><xdr:rowOff>0</xdr:rowOff></xdr:from>
    <xdr:to><xdr:col>8</xdr:col><xdr:colOff>0</xdr:colOff><xdr:row>22</xdr:row><xdr:rowOff>0</xdr:rowOff></xdr:to>
    <xdr:graphicFrame>
      <xdr:nvGraphicFramePr><xdr:cNvPr id="3" name="Report Chart" /><xdr:cNvGraphicFramePr /></xdr:nvGraphicFramePr>
      <xdr:xfrm><a:off x="0" y="0" /><a:ext cx="0" cy="0" /></xdr:xfrm>
      <a:graphic><a:graphicData uri="http://schemas.openxmlformats.org/drawingml/2006/chart"><c:chart xmlns:c="http://schemas.openxmlformats.org/drawingml/2006/chart" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships" r:id="rId1" /></a:graphicData></a:graphic>
    </xdr:graphicFrame>
    <xdr:clientData />
  </xdr:twoCellAnchor>
</xdr:wsDr>
'@
}

function New-ChartXml {
    param([string]$ReportTitle)

    return @"
<?xml version="1.0" encoding="utf-8"?>
<c:chartSpace xmlns:c="http://schemas.openxmlformats.org/drawingml/2006/chart" xmlns:a="http://schemas.openxmlformats.org/drawingml/2006/main">
  <c:lang val="en-US" />
  <c:chart>
    <c:title><c:tx><c:rich><a:bodyPr /><a:lstStyle /><a:p><a:r><a:t>$ReportTitle</a:t></a:r></a:p></c:rich></c:tx></c:title>
    <c:plotArea>
      <c:layout />
      <c:barChart>
        <c:barDir val="col" />
        <c:grouping val="clustered" />
        <c:ser>
          <c:idx val="0" />
          <c:order val="0" />
          <c:tx><c:v>$ReportTitle</c:v></c:tx>
          <c:cat><c:strRef><c:f>'Graph'!`$A`$4:`$A`$4</c:f></c:strRef></c:cat>
          <c:val><c:numRef><c:f>'Graph'!`$B`$4:`$B`$4</c:f></c:numRef></c:val>
        </c:ser>
        <c:axId val="48650112" />
        <c:axId val="48672768" />
      </c:barChart>
      <c:catAx><c:axId val="48650112" /><c:scaling><c:orientation val="minMax" /></c:scaling><c:axPos val="b" /><c:crossAx val="48672768" /><c:crosses val="autoZero" /></c:catAx>
      <c:valAx><c:axId val="48672768" /><c:scaling><c:orientation val="minMax" /></c:scaling><c:axPos val="l" /><c:crossAx val="48650112" /><c:crosses val="autoZero" /><c:crossBetween val="between" /></c:valAx>
    </c:plotArea>
    <c:legend><c:legendPos val="b" /><c:overlay val="0" /></c:legend>
    <c:plotVisOnly val="1" />
  </c:chart>
</c:chartSpace>
"@
}

function New-CoreXml {
    param([string]$ReportTitle)
    $timestamp = [DateTime]::UtcNow.ToString('yyyy-MM-ddTHH:mm:ssZ', [Globalization.CultureInfo]::InvariantCulture)
    return @"
<?xml version="1.0" encoding="utf-8"?>
<cp:coreProperties xmlns:cp="http://schemas.openxmlformats.org/package/2006/metadata/core-properties" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:dcterms="http://purl.org/dc/terms/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <dc:title>$ReportTitle</dc:title>
  <dc:creator>Academic Information System</dc:creator>
  <dcterms:created xsi:type="dcterms:W3CDTF">$timestamp</dcterms:created>
  <dcterms:modified xsi:type="dcterms:W3CDTF">$timestamp</dcterms:modified>
</cp:coreProperties>
"@
}

function New-AppXml {
    return @'
<?xml version="1.0" encoding="utf-8"?>
<Properties xmlns="http://schemas.openxmlformats.org/officeDocument/2006/extended-properties">
  <Application>Academic Information System</Application>
  <TitlesOfParts><vt:vector xmlns:vt="http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes" size="2" baseType="lpstr"><vt:lpstr>Report</vt:lpstr><vt:lpstr>Graph</vt:lpstr></vt:vector></TitlesOfParts>
</Properties>
'@
}

function New-Template {
    param(
        [string]$OutputPath,
        [string]$ReportTitle
    )

    if (Test-Path $OutputPath) {
        Remove-Item -LiteralPath $OutputPath
    }

    $package = [System.IO.Packaging.Package]::Open($OutputPath, [System.IO.FileMode]::CreateNew)
    try {
        $workbookPart = Write-Part $package '/xl/workbook.xml' 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml' (New-WorkbookXml $ReportTitle)
        Write-Part $package '/xl/styles.xml' 'application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml' (New-StylesXml) | Out-Null
        $sheet1Part = Write-Part $package '/xl/worksheets/sheet1.xml' 'application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml' (New-Sheet1Xml $ReportTitle)
        $sheet2Part = Write-Part $package '/xl/worksheets/sheet2.xml' 'application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml' (New-Sheet2Xml $ReportTitle)
        $logoDrawingPart = Write-Part $package '/xl/drawings/drawing1.xml' 'application/vnd.openxmlformats-officedocument.drawing+xml' (New-LogoDrawingXml)
        $chartDrawingPart = Write-Part $package '/xl/drawings/drawing2.xml' 'application/vnd.openxmlformats-officedocument.drawing+xml' (New-ChartDrawingXml)
        $chartPart = Write-Part $package '/xl/charts/chart1.xml' 'application/vnd.openxmlformats-officedocument.drawingml.chart+xml' (New-ChartXml $ReportTitle)
        Write-Part $package '/docProps/core.xml' 'application/vnd.openxmlformats-package.core-properties+xml' (New-CoreXml $ReportTitle) | Out-Null
        Write-Part $package '/docProps/app.xml' 'application/vnd.openxmlformats-officedocument.extended-properties+xml' (New-AppXml) | Out-Null

        if (Test-Path $logoPath) {
            Add-BinaryPart $package '/xl/media/company-logo.png' 'image/png' $logoPath | Out-Null
            $logoDrawingPart.CreateRelationship([Uri]::new('../media/company-logo.png', [UriKind]::Relative), [System.IO.Packaging.TargetMode]::Internal, 'http://schemas.openxmlformats.org/officeDocument/2006/relationships/image', 'rId1') | Out-Null
        }

        $package.CreateRelationship([Uri]::new('/xl/workbook.xml', [UriKind]::Relative), [System.IO.Packaging.TargetMode]::Internal, 'http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument', 'rId1') | Out-Null
        $package.CreateRelationship([Uri]::new('/docProps/core.xml', [UriKind]::Relative), [System.IO.Packaging.TargetMode]::Internal, 'http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties', 'rId2') | Out-Null
        $package.CreateRelationship([Uri]::new('/docProps/app.xml', [UriKind]::Relative), [System.IO.Packaging.TargetMode]::Internal, 'http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties', 'rId3') | Out-Null

        $workbookPart.CreateRelationship([Uri]::new('worksheets/sheet1.xml', [UriKind]::Relative), [System.IO.Packaging.TargetMode]::Internal, 'http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet', 'rId1') | Out-Null
        $workbookPart.CreateRelationship([Uri]::new('worksheets/sheet2.xml', [UriKind]::Relative), [System.IO.Packaging.TargetMode]::Internal, 'http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet', 'rId2') | Out-Null
        $workbookPart.CreateRelationship([Uri]::new('styles.xml', [UriKind]::Relative), [System.IO.Packaging.TargetMode]::Internal, 'http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles', 'rId3') | Out-Null

        $sheet1Part.CreateRelationship([Uri]::new('../drawings/drawing1.xml', [UriKind]::Relative), [System.IO.Packaging.TargetMode]::Internal, 'http://schemas.openxmlformats.org/officeDocument/2006/relationships/drawing', 'rId1') | Out-Null
        $sheet2Part.CreateRelationship([Uri]::new('../drawings/drawing2.xml', [UriKind]::Relative), [System.IO.Packaging.TargetMode]::Internal, 'http://schemas.openxmlformats.org/officeDocument/2006/relationships/drawing', 'rId1') | Out-Null
        $chartDrawingPart.CreateRelationship([Uri]::new('../charts/chart1.xml', [UriKind]::Relative), [System.IO.Packaging.TargetMode]::Internal, 'http://schemas.openxmlformats.org/officeDocument/2006/relationships/chart', 'rId1') | Out-Null
    }
    finally {
        $package.Close()
    }
}

$templates = @(
    @{ File = 'Interop_EnrollmentTransactionsTemplate.xlsx'; Title = 'Enrollment Transactions' },
    @{ File = 'Interop_DroppedEnrollmentsTemplate.xlsx'; Title = 'Dropped Enrollments' },
    @{ File = 'Interop_GradesReportTemplate.xlsx'; Title = 'Grades Report' }
)

foreach ($template in $templates) {
    New-Template -OutputPath (Join-Path $templateDir $template.File) -ReportTitle $template.Title
}
