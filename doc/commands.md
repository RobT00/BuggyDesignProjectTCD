# Command specifications

Each message consists if a sender ID, a reciever ID, and a command. They are
terminated by a newline (`n`).

IDs can be 0, 1, or 2, with 0 representing the station, and the other two the
buggies

### Commands sent by the buggy:

Command | Explanation
--- | ---
PONG | Response to PING
PING | Response to PONG
GANTRY1 | Passing under Gantry 1
GANTRY2 | Passing under Gantry 2
GANTRY3 | Passing under Gantry 3
PARKED | When the buggy has stopped after being requested to park with a PARK
OBSTACLE | Informs the station that an obstacle has appeared in the path of the buggy and therefore it has temporarily stopped
PATHCLEAR | Informs the station that the obstacle has been removed and the buggy is resuming travel

### Commands sent by the station:

Command | Explanation
--- | ---
PING | For communications testing, expecting PONG
PONG | For communications testing, expecting PING
LED | Requests buggy to flash its LED for identification purposes
CLOCK | Informs buggy that it will go clockwise around the track (will be relevant for levels silver and gold)
ACLOCK | Informs buggy that it will go anti-clockwise around the track (will be relevant for levels silver and gold)
GO | Commands buggy to go, following line
STOP | Commands buggy to stop immediately
STRAIGHT | Commands buggy to go straight if it encounters an intersection
PARK | Commands buggy to turn if it encounters an intersection. Overridden side depends on the orientation sent earlier (clockwise vs anti-clockwise)