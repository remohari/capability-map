param(
    [string]$SourcePath = "C:\AgenticDays\digiberatung\DigiDude\.claude\commands",
    [string]$OutputPath = "skills"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Convert-ToSkillName {
    param([string]$BaseName)

    $normalized = $BaseName.ToLowerInvariant()
    $normalized = $normalized -replace "[^a-z0-9]+", "-"
    $normalized = $normalized.Trim("-")

    if ([string]::IsNullOrWhiteSpace($normalized)) {
        throw "Cannot derive a valid skill name from '$BaseName'."
    }

    return $normalized
}

function Split-Frontmatter {
    param([string]$RawText)

    if (-not $RawText.StartsWith("---")) {
        return @{
            Frontmatter = @{}
            Body = $RawText.TrimStart("`r", "`n")
        }
    }

    $match = [regex]::Match(
        $RawText,
        "^(---\r?\n)([\s\S]*?)(\r?\n---\r?\n?)([\s\S]*)$"
    )

    if (-not $match.Success) {
        return @{
            Frontmatter = @{}
            Body = $RawText.TrimStart("`r", "`n")
        }
    }

    $frontmatterText = $match.Groups[2].Value
    $body = $match.Groups[4].Value.TrimStart("`r", "`n")
    $frontmatter = @{}

    foreach ($line in ($frontmatterText -split "\r?\n")) {
        if ($line -match "^\s*([A-Za-z0-9_-]+)\s*:\s*(.*)\s*$") {
            $key = $matches[1]
            $value = $matches[2].Trim()

            if ($value.StartsWith("'") -and $value.EndsWith("'")) {
                $value = $value.Substring(1, $value.Length - 2)
            } elseif ($value.StartsWith('"') -and $value.EndsWith('"')) {
                $value = $value.Substring(1, $value.Length - 2)
            }

            $frontmatter[$key] = $value
        }
    }

    return @{
        Frontmatter = $frontmatter
        Body = $body
    }
}

function Convert-ToYamlSingleQuoted {
    param([string]$Value)

    if ($null -eq $Value) {
        $Value = ""
    }

    return "'" + ($Value -replace "'", "''") + "'"
}

if (-not (Test-Path -LiteralPath $SourcePath)) {
    throw "Source path not found: $SourcePath"
}

New-Item -ItemType Directory -Force -Path $OutputPath | Out-Null

$generated = @()
$files = Get-ChildItem -LiteralPath $SourcePath -File -Filter *.md | Sort-Object Name

foreach ($file in $files) {
    $skillName = Convert-ToSkillName -BaseName $file.BaseName
    $parsed = Split-Frontmatter -RawText (Get-Content -LiteralPath $file.FullName -Raw)
    $description = $parsed.Frontmatter["description"]

    if ([string]::IsNullOrWhiteSpace($description)) {
        $description = "Imported Claude command workflow from $($file.Name). Use when Codex should execute the instructions defined by this command."
    }

    $skillDir = Join-Path $OutputPath $skillName
    New-Item -ItemType Directory -Force -Path $skillDir | Out-Null

    $skillText = @(
        "---"
        "name: $(Convert-ToYamlSingleQuoted -Value $skillName)"
        "description: $(Convert-ToYamlSingleQuoted -Value $description)"
        "---"
        ""
        $parsed.Body.TrimEnd()
        ""
    ) -join "`r`n"

    Set-Content -LiteralPath (Join-Path $skillDir "SKILL.md") -Value $skillText -NoNewline
    $generated += $skillName
}

$generated | ForEach-Object { $_ }
