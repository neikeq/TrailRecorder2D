TrailRecorder2D
===============

Unity tool for recording trail snapshots of 2d GameObjects

This is a simple tool I made to have a snapshot of my player's trail during the last play.
It was useful for easily checking where I had place floors or walls.<br>
The idea is taken from the presentation of Mario Maker at E3 2014.

### Requires

Unity 4.3 or higher.

### Usage

Just add the **TrailRecorder** script to the GameObject you want to record its trail from.

You can set the snapshots's limit to avoid overburdening your scene with too many snapshots (or leave it as zero ignore this limit).

The **visible** field determines if each snapshot must be instantiated after recording it.

You can clear the recorded snapshots with the **Clear all** button (this will also destroy all the instances in the scene).

All snapshots instances are placed as child of a container GameObject to make it easier to destroy them.

### Limitations

- Currently it can only record snapshots from a single GameObject at the same time.
- Transitions between play mode are only handled if method OnEnable in our TrailRecorderEditor is called.

