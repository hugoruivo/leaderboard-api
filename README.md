# leaderboard-api

### Setup

This API uses MongoDB to store data, you will need to edit the `appsettings.json` file with your MongoDB config.

You only need to edit the following keys:

`ConnectionString` -> add your MongoDB connection string.

`DatabaseName` -> add your MongoDB database.


### API Endpoints:

##### 1. Get all leaderboard scores and usernames with pagination

To get all the scores, with pagination, it is a GET method:

`<base_url>/api/scores?page=1`

This endpoints expects 1 parameter which is the current page requested, it internally uses 2 items per page which can be changed in the `ScoreService`class updating the constant `ITEMS_PER_PAGE`.

##### 2. Register Username

In order to submit a score you need to register first using the following endpoint:

`<base_url>/api/user`

You need to send the following parameter by POST:
```
{
    "UserName": "username to register"
}
```

##### 3. Register New Score

To register a new user score:

`<base_url>/api/scores`

You need to send the following parameter by POST:
```
{
    "UserName": "username the score belongs to",
    "UserScore": <int score>
}
```
