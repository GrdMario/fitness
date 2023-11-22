
```
{
  "MssqlSettings:ConnectionString": "Server=localhost; Database=Fitness;User Id=SA;Password=yourStrong(!)Password;TrustServerCertificate=Yes",
  "BlobSettings": {
    "Url": "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"
  },
  "SecurityAdapterSettings": {
    "JwtTokenSettings": {
      "Issuer": "https://localhost:5001/",
      "Audience": "https://localhost:5001/",
      "Secret": "mysupersecretpassword",
      "ExpiresInMinutes": 20
    },
    "RefreshTokenSettings": {
      "ValidForDays": 30
    },
    "PasswordSettings": {
      "RequiredLength": 10,
      "RequireNonLetterOrDigit": true,
      "RequireLowercase": true,
      "RequireUppercase": true,
      "RequireDigit": true
    }
    }
}
```