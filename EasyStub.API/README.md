# EasyStub.API

## GET /\_admin/endpoints

Will return all of the configured endpoints

## POST /\_admin/endpoints

Will create a new endpoint with the content:

```json
{
    "path": "/example"
}
```

## GET /\_admin/fallback

Will return the configured fallback method. One of:

```json
{
    "type": "NOT_FOUND"
}
```

This will simply return a 404

```json
{
    "type": "JSON",
    "statusCode": 200,
    "value": {...}
}
```

This will return the specified statusCode

## POST /\_admin/fallback

Will define
