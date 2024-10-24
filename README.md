# ExecutionLens API Documentation

The **ExecutionLens API** provides a variety of endpoints to handle execution logs, reproduce API flows, create charts, and perform searches on logs. The API also offers features for exporting data and analyzing execution patterns.

# Endpoints Overview

1. **Charts**
   - POST /Chart/GetExceptionsCount
   - POST /Chart/GetExecutionsTimes
   - GET /Chart/GetLogExecutionsTime/{Id}
   - POST /Chart/GetRequestsCount

2. **Log Operations**
   - GET /Log/GetClassNames
   - GET /Log/{Id}
   - POST /Log/GetMethodNames
   - GET /Log/GetMethodExceptions
   - GET /Log/OverviewNode/{NodeId}

3. **Search Operations**
   - POST /Search/NLPSearchNodes
   - POST /Search/Save
   - DELETE /Search/{Id}
   - GET /Search
   - POST /Search/SearchNodes

4. **Export**
   - POST /Export/Nodes
