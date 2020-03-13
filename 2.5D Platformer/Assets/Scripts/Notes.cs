using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour
{
    //TODO
    //Finish tutorial on curves, figure out which kind to use.
    //Use a combination of Lerp and the Splines to make path for player, make a worldPosition function on spline
    //Player follows path on forward press
    //Spline should stay at fixed level and player height should be determined by ground
    //Forward moves them to the next point

    //THE PLAN: List of waypoints. directions move player along spline until their position equals the waypoint they are moving towards. 
    //Then they move to the next waypoint. Levels will have their own list of bools and floats to set looping and resolution for each curve.
    //Options based on performance: either always draw lines or only when player is on that line's area. Probably Second One.
    //Maybe implement a "world position" function, break up curves and add to points, then get closest one. Resolution needs to be high.
    //Ground's tag is what spline they correspond to, cross-reference with waypoints. Decide which should trigger change; collision with ground or position equalling waypoint position. Probably First One.
    //When a player enters a new ground (OnCollisionEnter), set their position to the nearest world position point, and start calculating line. MAKE SURE ONLY BETWEEN TOP OF GROUND AND BOTTOM OF PLAYER.
    //Ground's layer determines camera controls. 

    //PLAN REVISED: Player's ground has a tag that states what curve they are following, curve is broken into the points of its resolution
    //Directions move player in direction of next point on curve, until they hit a new ground
    //List of waypoints, list contains the waypoint and its curves
    //2D, y position ignored and irrelevant, player moved in y by physics. 

    //Camera follows player at certain angles determined by tag of ground
}
