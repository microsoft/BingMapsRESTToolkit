param(
	[string]$apiKey="", 
	[string]$certIdentifier="",
	[string]$certIdType="subject")

Write-Host "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
Write-Host "RecursiveGeek BingMapsRESTToolkit.Standard NuGet Deployment Tool"
Write-Host "Developed by Hans Dickel"
Write-Host "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"

Write-Host "This tool takes the released version of a NuGet package, digitally signs it (via subject or fingerprint), and"
Write-Host "Posts it to NuGet.org."
Write-Host ""

# Digitally sign this PowerShell script 
# Using Package Manager Console or PowerShell console:  & "$Env:SIGNTOOL" sign /n "Daikin Applied Americas Inc." /t "http://timestamp.verisign.com/scripts/timstamp.dll" "PublishNuget.ps1"
# Using Command Prompt:                                 "%SIGNTOOL%" sign /n "Daikin Applied Americas Inc." /t "http://timestamp.verisign.com/scripts/timstamp.dll" "PublishNuget.ps1"

# Digitally Signing Timestamp Server for NuGet Packages (nupkg)
$timeServer="http://timestamp.comodoca.com?td=sha256"

Function PublishNuGet
{
    param ([string]$project)

	Write-Output "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
    Write-Output "~~ Publish: $project ~~"

	Try
    {
        $newestPackage = Get-ChildItem $project\bin\Release\*.nupkg -File -ErrorAction Stop | Sort-Object LastAccessTime -Descending | Select-Object -First 1
    }
    Catch [System.Exception]
    {
        $newestPackage = $null
    }

    if ($newestPackage)
    {
		if ($certIdType.ToLower() -eq "subject")
		{
			nuget sign $newestPackage.FullName -CertificateSubjectName $certIdentifier -Timestamper $timeServer
		}
		else
		{
			nuget sign $newestPackage.FullName -CertificateFingerprint $certIdentifier -Timestamper $timeServer
		}
		
        nuget push $newestPackage.FullName -Source https://www.nuget.org/api/v2/package -ApiKey $apiKey
    }
    else
    {
        Write-Output "Unable to find a package to deploy"
    } 
	Write-Output ""
}

if ($apiKey.Length -eq 0)
{
	$apiKey = Read-Host -Prompt 'Enter your NuGet Api Key'
}
else
{
	Write-Host "(Buget Api Key passed via command-line)"
}

if ($certIdentifier.Length -eq 0)
{
	$certIdentifier = Read-Host -Prompt "Enter your NuGet Certificate Identifier ($certIdType)"
}
else
{
	Write-Host "(NuGet Certificate $certIdType passed via command-line)"
}

if ($apiKey.Length -ne 0)
{
	PublishNuGet "Source\BingMapsRESTToolkit.Standard"
}
else
{
	Write-Host "Api Key not specified.  Discontinuing."
}

# ~End~

# SIG # Begin signature block
# MIIXtwYJKoZIhvcNAQcCoIIXqDCCF6QCAQExCzAJBgUrDgMCGgUAMGkGCisGAQQB
# gjcCAQSgWzBZMDQGCisGAQQBgjcCAR4wJgIDAQAABBAfzDtgWUsITrck0sYpfvNR
# AgEAAgEAAgEAAgEAAgEAMCEwCQYFKw4DAhoFAAQUJ0rgLAkQAgZiTqkRlcE/pZDy
# KxSgghLlMIID7jCCA1egAwIBAgIQfpPr+3zGTlnqS5p31Ab8OzANBgkqhkiG9w0B
# AQUFADCBizELMAkGA1UEBhMCWkExFTATBgNVBAgTDFdlc3Rlcm4gQ2FwZTEUMBIG
# A1UEBxMLRHVyYmFudmlsbGUxDzANBgNVBAoTBlRoYXd0ZTEdMBsGA1UECxMUVGhh
# d3RlIENlcnRpZmljYXRpb24xHzAdBgNVBAMTFlRoYXd0ZSBUaW1lc3RhbXBpbmcg
# Q0EwHhcNMTIxMjIxMDAwMDAwWhcNMjAxMjMwMjM1OTU5WjBeMQswCQYDVQQGEwJV
# UzEdMBsGA1UEChMUU3ltYW50ZWMgQ29ycG9yYXRpb24xMDAuBgNVBAMTJ1N5bWFu
# dGVjIFRpbWUgU3RhbXBpbmcgU2VydmljZXMgQ0EgLSBHMjCCASIwDQYJKoZIhvcN
# AQEBBQADggEPADCCAQoCggEBALGss0lUS5ccEgrYJXmRIlcqb9y4JsRDc2vCvy5Q
# WvsUwnaOQwElQ7Sh4kX06Ld7w3TMIte0lAAC903tv7S3RCRrzV9FO9FEzkMScxeC
# i2m0K8uZHqxyGyZNcR+xMd37UWECU6aq9UksBXhFpS+JzueZ5/6M4lc/PcaS3Er4
# ezPkeQr78HWIQZz/xQNRmarXbJ+TaYdlKYOFwmAUxMjJOxTawIHwHw103pIiq8r3
# +3R8J+b3Sht/p8OeLa6K6qbmqicWfWH3mHERvOJQoUvlXfrlDqcsn6plINPYlujI
# fKVOSET/GeJEB5IL12iEgF1qeGRFzWBGflTBE3zFefHJwXECAwEAAaOB+jCB9zAd
# BgNVHQ4EFgQUX5r1blzMzHSa1N197z/b7EyALt0wMgYIKwYBBQUHAQEEJjAkMCIG
# CCsGAQUFBzABhhZodHRwOi8vb2NzcC50aGF3dGUuY29tMBIGA1UdEwEB/wQIMAYB
# Af8CAQAwPwYDVR0fBDgwNjA0oDKgMIYuaHR0cDovL2NybC50aGF3dGUuY29tL1Ro
# YXd0ZVRpbWVzdGFtcGluZ0NBLmNybDATBgNVHSUEDDAKBggrBgEFBQcDCDAOBgNV
# HQ8BAf8EBAMCAQYwKAYDVR0RBCEwH6QdMBsxGTAXBgNVBAMTEFRpbWVTdGFtcC0y
# MDQ4LTEwDQYJKoZIhvcNAQEFBQADgYEAAwmbj3nvf1kwqu9otfrjCR27T4IGXTdf
# plKfFo3qHJIJRG71betYfDDo+WmNI3MLEm9Hqa45EfgqsZuwGsOO61mWAK3ODE2y
# 0DGmCFwqevzieh1XTKhlGOl5QGIllm7HxzdqgyEIjkHq3dlXPx13SYcqFgZepjhq
# IhKjURmDfrYwggSjMIIDi6ADAgECAhAOz/Q4yP6/NW4E2GqYGxpQMA0GCSqGSIb3
# DQEBBQUAMF4xCzAJBgNVBAYTAlVTMR0wGwYDVQQKExRTeW1hbnRlYyBDb3Jwb3Jh
# dGlvbjEwMC4GA1UEAxMnU3ltYW50ZWMgVGltZSBTdGFtcGluZyBTZXJ2aWNlcyBD
# QSAtIEcyMB4XDTEyMTAxODAwMDAwMFoXDTIwMTIyOTIzNTk1OVowYjELMAkGA1UE
# BhMCVVMxHTAbBgNVBAoTFFN5bWFudGVjIENvcnBvcmF0aW9uMTQwMgYDVQQDEytT
# eW1hbnRlYyBUaW1lIFN0YW1waW5nIFNlcnZpY2VzIFNpZ25lciAtIEc0MIIBIjAN
# BgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAomMLOUS4uyOnREm7Dv+h8GEKU5Ow
# mNutLA9KxW7/hjxTVQ8VzgQ/K/2plpbZvmF5C1vJTIZ25eBDSyKV7sIrQ8Gf2Gi0
# jkBP7oU4uRHFI/JkWPAVMm9OV6GuiKQC1yoezUvh3WPVF4kyW7BemVqonShQDhfu
# ltthO0VRHc8SVguSR/yrrvZmPUescHLnkudfzRC5xINklBm9JYDh6NIipdC6Anqh
# d5NbZcPuF3S8QYYq3AhMjJKMkS2ed0QfaNaodHfbDlsyi1aLM73ZY8hJnTrFxeoz
# C9Lxoxv0i77Zs1eLO94Ep3oisiSuLsdwxb5OgyYI+wu9qU+ZCOEQKHKqzQIDAQAB
# o4IBVzCCAVMwDAYDVR0TAQH/BAIwADAWBgNVHSUBAf8EDDAKBggrBgEFBQcDCDAO
# BgNVHQ8BAf8EBAMCB4AwcwYIKwYBBQUHAQEEZzBlMCoGCCsGAQUFBzABhh5odHRw
# Oi8vdHMtb2NzcC53cy5zeW1hbnRlYy5jb20wNwYIKwYBBQUHMAKGK2h0dHA6Ly90
# cy1haWEud3Muc3ltYW50ZWMuY29tL3Rzcy1jYS1nMi5jZXIwPAYDVR0fBDUwMzAx
# oC+gLYYraHR0cDovL3RzLWNybC53cy5zeW1hbnRlYy5jb20vdHNzLWNhLWcyLmNy
# bDAoBgNVHREEITAfpB0wGzEZMBcGA1UEAxMQVGltZVN0YW1wLTIwNDgtMjAdBgNV
# HQ4EFgQURsZpow5KFB7VTNpSYxc/Xja8DeYwHwYDVR0jBBgwFoAUX5r1blzMzHSa
# 1N197z/b7EyALt0wDQYJKoZIhvcNAQEFBQADggEBAHg7tJEqAEzwj2IwN3ijhCcH
# bxiy3iXcoNSUA6qGTiWfmkADHN3O43nLIWgG2rYytG2/9CwmYzPkSWRtDebDZw73
# BaQ1bHyJFsbpst+y6d0gxnEPzZV03LZc3r03H0N45ni1zSgEIKOq8UvEiCmRDoDR
# EfzdXHZuT14ORUZBbg2w6jiasTraCXEQ/Bx5tIB7rGn0/Zy2DBYr8X9bCT2bW+IW
# yhOBbQAuOA2oKY8s4bL0WqkBrxWcLC9JG9siu8P+eJRRw4axgohd8D20UaF5Mysu
# e7ncIAkTcetqGVvP6KUwVyyJST+5z3/Jvz4iaGNTmr1pdKzFHTx/kuDDvBzYBHUw
# ggTrMIID06ADAgECAhALBwb6blv+e5qBU96K6YsbMA0GCSqGSIb3DQEBCwUAMH8x
# CzAJBgNVBAYTAlVTMR0wGwYDVQQKExRTeW1hbnRlYyBDb3Jwb3JhdGlvbjEfMB0G
# A1UECxMWU3ltYW50ZWMgVHJ1c3QgTmV0d29yazEwMC4GA1UEAxMnU3ltYW50ZWMg
# Q2xhc3MgMyBTSEEyNTYgQ29kZSBTaWduaW5nIENBMB4XDTE3MTIxMTAwMDAwMFoX
# DTIxMDEwODIzNTk1OVowgYIxCzAJBgNVBAYTAlVTMRIwEAYDVQQIDAlNaW5uZXNv
# dGExETAPBgNVBAcMCFBseW1vdXRoMSUwIwYDVQQKDBxEYWlraW4gQXBwbGllZCBB
# bWVyaWNhcyBJbmMuMSUwIwYDVQQDDBxEYWlraW4gQXBwbGllZCBBbWVyaWNhcyBJ
# bmMuMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA2oWIM1UdWlb/Un8j
# nXMpL4c/JvCp2YRrqcoA/4zIm48js473SeBwXa7npDe+9bkOX4iGaO3CAKwNonuX
# eHn2GOBhTfg0H3hBzBrby+SqaXXTVP81OP7j3qZheIRK2YYf5WmiIblXF9vc0j8e
# pEhUyqr8A5C2RO3s3T7OHlAD8W2VOG9DQnzRKOrn+ysQ/9C4gD00jrVk+8KLoaxX
# b6qL99UBRpQc13Fc/WPJ26nTnFFboqEhCSGy4RsM3nAAqWqpH9eo/vloIOE2qcdP
# +i9iC8JyLln58LgubmjdLWu/UUEoYcIEBbW/YaejaA+lGywIu+ObxjH2QWZNuoDU
# PnjRFwIDAQABo4IBXTCCAVkwCQYDVR0TBAIwADAOBgNVHQ8BAf8EBAMCB4AwKwYD
# VR0fBCQwIjAgoB6gHIYaaHR0cDovL3N2LnN5bWNiLmNvbS9zdi5jcmwwYQYDVR0g
# BFowWDBWBgZngQwBBAEwTDAjBggrBgEFBQcCARYXaHR0cHM6Ly9kLnN5bWNiLmNv
# bS9jcHMwJQYIKwYBBQUHAgIwGQwXaHR0cHM6Ly9kLnN5bWNiLmNvbS9ycGEwEwYD
# VR0lBAwwCgYIKwYBBQUHAwMwVwYIKwYBBQUHAQEESzBJMB8GCCsGAQUFBzABhhNo
# dHRwOi8vc3Yuc3ltY2QuY29tMCYGCCsGAQUFBzAChhpodHRwOi8vc3Yuc3ltY2Iu
# Y29tL3N2LmNydDAfBgNVHSMEGDAWgBSWO1PweTOXr32D7y4rzMq3hh5yZjAdBgNV
# HQ4EFgQUuDLTyQJoiY8jcDS/amxy75tcQAUwDQYJKoZIhvcNAQELBQADggEBADbY
# D4OXHKMfwjinMvK80Hccs6ud+eUgK1/D+GJn2QpiuHvMKMzuI22I41VZkViMEDC0
# M83XDKkIb1nesVXMS97yJHeYdyIkTNYI23kNRo9hERDBp1tWK7sXCGlY0soQ/kwQ
# r11cx0tQuNrTaG9e4PLhMcvU8iCwpWl5dWjtevUflmVhMVaqecdlQlumBBs/HXxF
# 4xSivMlWxtTGmDQtHf4uSUUcz6AYh2qnH8mvFXJ6uscWsHuSQsX8cXbrkcuk8cr4
# fPF1txiciwKDXmmGeZwKwh4wfGguszbsmZXSwPzluCaVBIP5LE+LN4zstvscO/T4
# moF3leUqnoBSNzgwU24wggVZMIIEQaADAgECAhA9eNf5dklgsmF99PAeyoYqMA0G
# CSqGSIb3DQEBCwUAMIHKMQswCQYDVQQGEwJVUzEXMBUGA1UEChMOVmVyaVNpZ24s
# IEluYy4xHzAdBgNVBAsTFlZlcmlTaWduIFRydXN0IE5ldHdvcmsxOjA4BgNVBAsT
# MShjKSAyMDA2IFZlcmlTaWduLCBJbmMuIC0gRm9yIGF1dGhvcml6ZWQgdXNlIG9u
# bHkxRTBDBgNVBAMTPFZlcmlTaWduIENsYXNzIDMgUHVibGljIFByaW1hcnkgQ2Vy
# dGlmaWNhdGlvbiBBdXRob3JpdHkgLSBHNTAeFw0xMzEyMTAwMDAwMDBaFw0yMzEy
# MDkyMzU5NTlaMH8xCzAJBgNVBAYTAlVTMR0wGwYDVQQKExRTeW1hbnRlYyBDb3Jw
# b3JhdGlvbjEfMB0GA1UECxMWU3ltYW50ZWMgVHJ1c3QgTmV0d29yazEwMC4GA1UE
# AxMnU3ltYW50ZWMgQ2xhc3MgMyBTSEEyNTYgQ29kZSBTaWduaW5nIENBMIIBIjAN
# BgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAl4MeABavLLHSCMTXaJNRYB5x9uJH
# tNtYTSNiarS/WhtR96MNGHdou9g2qy8hUNqe8+dfJ04LwpfICXCTqdpcDU6kDZGg
# tOwUzpFyVC7Oo9tE6VIbP0E8ykrkqsDoOatTzCHQzM9/m+bCzFhqghXuPTbPHMWX
# BySO8Xu+MS09bty1mUKfS2GVXxxw7hd924vlYYl4x2gbrxF4GpiuxFVHU9mzMtah
# DkZAxZeSitFTp5lbhTVX0+qTYmEgCscwdyQRTWKDtrp7aIIx7mXK3/nVjbI13Iwr
# b2pyXGCEnPIMlF7AVlIASMzT+KV93i/XE+Q4qITVRrgThsIbnepaON2b2wIDAQAB
# o4IBgzCCAX8wLwYIKwYBBQUHAQEEIzAhMB8GCCsGAQUFBzABhhNodHRwOi8vczIu
# c3ltY2IuY29tMBIGA1UdEwEB/wQIMAYBAf8CAQAwbAYDVR0gBGUwYzBhBgtghkgB
# hvhFAQcXAzBSMCYGCCsGAQUFBwIBFhpodHRwOi8vd3d3LnN5bWF1dGguY29tL2Nw
# czAoBggrBgEFBQcCAjAcGhpodHRwOi8vd3d3LnN5bWF1dGguY29tL3JwYTAwBgNV
# HR8EKTAnMCWgI6Ahhh9odHRwOi8vczEuc3ltY2IuY29tL3BjYTMtZzUuY3JsMB0G
# A1UdJQQWMBQGCCsGAQUFBwMCBggrBgEFBQcDAzAOBgNVHQ8BAf8EBAMCAQYwKQYD
# VR0RBCIwIKQeMBwxGjAYBgNVBAMTEVN5bWFudGVjUEtJLTEtNTY3MB0GA1UdDgQW
# BBSWO1PweTOXr32D7y4rzMq3hh5yZjAfBgNVHSMEGDAWgBR/02Wnwt3su/AwCfND
# OfoCrzMxMzANBgkqhkiG9w0BAQsFAAOCAQEAE4UaHmmpN/egvaSvfh1hU/6djF4M
# pnUeeBcj3f3sGgNVOftxlcdlWqeOMNJEWmHbcG/aIQXCLnO6SfHRk/5dyc1eA+CJ
# nj90Htf3OIup1s+7NS8zWKiSVtHITTuC5nmEFvwosLFH8x2iPu6H2aZ/pFalP62E
# LinefLyoqqM9BAHqupOiDlAiKRdMh+Q6EV/WpCWJmwVrL7TJAUwnewusGQUioGAV
# P9rJ+01Mj/tyZ3f9J5THujUOiEn+jf0or0oSvQ2zlwXeRAwV+jYrA9zBUAHxoRFd
# FOXivSdLVL4rhF4PpsN0BQrvl8OJIrEfd/O9zUPU8UypP7WLhK9k8tAUITGCBDww
# ggQ4AgEBMIGTMH8xCzAJBgNVBAYTAlVTMR0wGwYDVQQKExRTeW1hbnRlYyBDb3Jw
# b3JhdGlvbjEfMB0GA1UECxMWU3ltYW50ZWMgVHJ1c3QgTmV0d29yazEwMC4GA1UE
# AxMnU3ltYW50ZWMgQ2xhc3MgMyBTSEEyNTYgQ29kZSBTaWduaW5nIENBAhALBwb6
# blv+e5qBU96K6YsbMAkGBSsOAwIaBQCgcDAQBgorBgEEAYI3AgEMMQIwADAZBgkq
# hkiG9w0BCQMxDAYKKwYBBAGCNwIBBDAcBgorBgEEAYI3AgELMQ4wDAYKKwYBBAGC
# NwIBFTAjBgkqhkiG9w0BCQQxFgQUIQ1r35mxsmWMT649pBz/AkmrgvIwDQYJKoZI
# hvcNAQEBBQAEggEAHDlTZ8cjA3aYmP4TABsgQXR4mllYnf/yTOd0b0fLbQrf6H1L
# k8WzYPZ87g0caB5m1ZT3Y6zhPbQtvIXKZWTCvhzfsPf2Z1auH0kBd5p5inNiCVtR
# 83m28U6MivzNi6pd3kVR0O3htAnYaZUVdnH3KpGwirNekkBn32GIhXeljTZIY8qe
# aVbNomlrfuQLShsKL5aSSW19/5sbvtW48yhoBfkMmOis7SEx82/+BQmxhFjcscsE
# gMq1l41jGsFmP6Ucj9f47a50bgYLA7oDhqcKjK0lhGcmKhcScnkdR8tEHwHXx7JA
# 5H3wkr5XnqtQcPFQ7DEa4qfbe2QxQiETmHJRA6GCAgswggIHBgkqhkiG9w0BCQYx
# ggH4MIIB9AIBATByMF4xCzAJBgNVBAYTAlVTMR0wGwYDVQQKExRTeW1hbnRlYyBD
# b3Jwb3JhdGlvbjEwMC4GA1UEAxMnU3ltYW50ZWMgVGltZSBTdGFtcGluZyBTZXJ2
# aWNlcyBDQSAtIEcyAhAOz/Q4yP6/NW4E2GqYGxpQMAkGBSsOAwIaBQCgXTAYBgkq
# hkiG9w0BCQMxCwYJKoZIhvcNAQcBMBwGCSqGSIb3DQEJBTEPFw0xOTA2MDQxOTM4
# MDRaMCMGCSqGSIb3DQEJBDEWBBTXtigKdhp/ZxvVTVeggaCNPOk/YTANBgkqhkiG
# 9w0BAQEFAASCAQB+gg411204brNxlIOLuxxa5Ep9VngA02kv5NFY6E1EpVmLhGG6
# I3+mYX5Wh5osPux2J34hBmDCkW7DZD/b9p8lhnZGfNZkq9orQirnNHJcQxik2Rp8
# GdCC9OUpIbrSbHyY0Ays3vjz2MU9vBkKl89mmWPDcyzjzxx21qM6qy3ughlhWL5S
# 4VwGYDwNS0QmV6ktcN/zUCYZUGY6sOmY+41GREqECgvDBVzBnvMuDv/qAJJkVhxw
# NQYQxAcGXooeYkznqfF5hQmEL5hhQoRJxmjLo4aDALvzRArl0tQSJpgmkdPTe0Wn
# 54eJOf0eGF4/130nkxAPXonAXOH5x45i16pt
# SIG # End signature block
