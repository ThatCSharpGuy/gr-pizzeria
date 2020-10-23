```shell
python -m grpc_tools.protoc -I..\service_definitions\ --python_out=. --grpc_python_out=. ..\service_definitions\service.proto
```