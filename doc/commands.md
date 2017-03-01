# Command specifications

Each message consists of a sender ID, a receiver ID, and a command, separated by
a single space (` `). Messages are terminated by a newline (`\n`).

Valid IDs are `0`, `1` and `2`, where `0` represents the station and the other
two the buggies.

### Commands sent by the buggy to the station:

Command | Explanation
--- | ---
PONG | Response to PING
PING | Response to PONG
GOING | Buggy is going after receiveing a GO command
STOPPED | Buggy stopped after receiving a STOP command
GANTRY1 | Passing under Gantry 1
GANTRY2 | Passing under Gantry 2
GANTRY3 | Passing under Gantry 3
GANTRY_INVALID | The buggy detected a gantry, but its signature did not match any of the gantries
PARKED | When the buggy has stopped after being requested to park with a PARK command
OBSTACLE | Informs the station that an obstacle has appeared in the path of the buggy and therefore it has temporarily stopped
PATHCLEAR | Informs the station that the obstacle has been removed and the buggy is resuming travel
INVALID | An invalid message has been recieved from the station, addressed to this buggy

### Commands sent by the station to one of the buggies:

Command | Explanation
--- | ---
PING | For communications testing, expecting PONG
PONG | For communications testing, expecting PING
LED | Requests buggy to flash its LED for identification purposes
CLOCK | Informs buggy that it will go clockwise around the track
ACLOCK | Informs buggy that it will go anti-clockwise around the track
GO | Commands buggy to go, following the line
STOP | Commands buggy to stop immediately
PARK | Commands buggy to turn in the appropriate direction for the parking lane, then stop after parking
