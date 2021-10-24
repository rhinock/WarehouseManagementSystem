$script:BaseUri = "api"
$script:InvokeParams = @{
    ContentType = "application/json"
}

Function GetObjects {
    [cmdletbinding()]
    param(
        [Parameter(ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string]$Protocol = "https",
        [Parameter(ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string]$Host = "localhost",
        [Parameter(ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [uint16]$Port = 44385,

        [Parameter(Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string]$Objects,
        [Parameter(ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [int64]$Id
    )
    Begin {
        ## Using the module-scoped API endpoint URI to create a function-specific URL to query
        $Uri = "$Protocol" + "://" + "$Host" + ":" + "$Port/$script:BaseUri/$Objects"
        
        if ($Id -ne 0)
        {
            $Uri = $Uri + "/$Id";
        }
    }
    Process {
        # Call the API endpoint using the $Uri
        Invoke-RestMethod -Uri $Uri -Method Get @script:InvokeParams
    }
}

Function PostOrPutObject {
    [cmdletbinding()]
    param(
        [Parameter(ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string]$Protocol = "https",
        [Parameter(ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string]$Host = "localhost",
        [Parameter(ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [uint16]$Port = 44385,
 
        [Parameter(Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string]$Objects,
        [Parameter(Mandatory, ValueFromPipelineByPropertyName)]
        [string]$Method,
        [Parameter(Mandatory, ValueFromPipelineByPropertyName)]
        [string]$JsonFilePath
    )
    Begin {
        $Uri = "$Protocol" + "://" + "$Host" + ":" + "$Port/$script:BaseUri/$Objects"
    }
    Process {
        $BodyJson = Get-Content -Path $JsonFilePath

        # Call the API endpoint using the $TempUri
        Invoke-RestMethod -Uri $Uri -Method $Method -Body $BodyJson @InvokeParams
    }
}

Function DeleteObject {
    [cmdletbinding()]
    param(
        [Parameter(ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string]$Protocol = "https",
        [Parameter(ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string]$Host = "localhost",
        [Parameter(ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [uint16]$Port = 44385,
 
        [Parameter(ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string]$Objects = "WarehouseItems",
        [Parameter(Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [int64]$Id
    )
    Begin {
        $Uri = "$Protocol" + "://" + "$Host" + ":" + "$Port/$script:BaseUri/$Objects/$Id"
    }
    Process {
        # Call the API endpoint using the $TempUri
        Invoke-RestMethod -Uri $Uri -Method DELETE @script:InvokeParams
    }
}