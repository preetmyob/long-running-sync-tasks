#!/bin/bash
for i in {1..1000000}; do
    echo $i
    curl -X 'PUT' \
    'https://localhost:7206/api/My/UpdateRoute' \
    -H 'accept: */*' \
    -H 'Content-Type: application/json' \
    -d '0'
done
