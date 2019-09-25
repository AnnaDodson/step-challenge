```
curl -XPOST -H 'Content-Type: application/json' -H 'accept: application/json' -d 'query RootQueryType { User(id: "1"){name} }' https://localhost:5001/user -vik
```

```
{"query" : "{ user(id: 1) {userName, team {name}} }"}
```

```
{    "query": "{ team(id: 1) {name, users {userName}} }" }
```

```
 {    "query": "{ user(id: 1) {userName, team {name}, steps {stepCount, dateOfSteps, week} }}"}
```

```
{  "query" : "mutation creatStepsEntry { creatStepsEntry ( steps: { stepCount : 444 , dateOfSteps : '2019-09-04T00:00:00+00:00' , userId : 1 }){name} " }
```

