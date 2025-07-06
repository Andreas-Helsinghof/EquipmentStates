# EquipmentStates

Solution to simulate a very simple state handling of equipment in a production enviornment. 

The solution consists of three parts
A HMI frontend together with a API 
A simple OrdersAPI that can subscibe to the state of the equipment
And a OPCUA server that can affect the state of the equipment

## Running the APIs
In the root of the project run 
```
docker compose up --build
```
This will start 
2xHMI exposed at http://localhost:5001, and http://localhost:5002,
a OrdersAPI exposed at http://localhost:5003/,
and a OPCUA Server at opc.tcp://0.0.0.0:4840/freeopcua/server

Both the HMI and OrdersAPI exposes a swagger at /swagger/index.html
Furthermore a incomplete "HMI" can also be accessed at http://localhost:5001/equipment-state

## Key features
The HMI API exposes an endpoint that subscibes to changes in the state of the equipement (http://localhost:5002/api/equipmentStateSse/stream)
The OrdersAPI initiates a subsciption to a equipments state through the POST /EquipmentSSE/RegisterEquipmentState endpoint 

The state of the equipment can be updated through the POST qpi/equipmentstate endpoint

The HMI will subscibe to mocked data from the equipment, and this data will trigger state updates aswell. 


## Subscribing to the HMI SSE endpoint from OrdersAPI
After subscribing you should see the state changes in the log of the OrdersAPI, State changes should happen without further interaction due to the OPCUA events

```
curl -X 'POST' \
  'http://localhost:5003/EquipmentSSE/RegisterEquipmentState' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "equipmentId": "11111111-1111-1111-1111-111111111111",
  "ssEurl": "http://hmi1:5001/api/equipmentStateSse/stream"
}'
```

```
curl -X 'POST' \
  'http://localhost:5003/EquipmentSSE/RegisterEquipmentState' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "equipmentId": "22222222-2222-2222-2222-222222222222",
  "ssEurl": "http://hmi2:5002/api/equipmentStateSse/stream"
}'
```

## Future work
To conclude the DEMO future work could be worked upon:
- Make the HMI instantly react to state changes in the frontend
- The OrdersAPI could expose a proper model that should be used when communicating over SSE
- Another HMI project should be created leveraging the IMachineEvents interface in a different manner
   - The API project could be packaged and the IMachineEvents reused to show compatibility with other equipment
- The equipment could react to the details of the order, and which parts should be proccessed.
