# coatsink-keyframes-test
Test task for Coatsink

Keyframes Programming Test

Write a minimal Keyframe Array Manager capable of adding/removing keyframes, searching through them, and interpolating between two keyframes.
A test script (Program.cs) has been provided for you to edit. This task is designed to take around 120 minutes, although this is not a hard limit should you need longer.

Please implement the following
1. This class is a mess, reorganise the code.
  - Assume Program is your game and we’d like clean access to the keyframe array functionality.
  - The program must still add 1,000,000 frames, remove 100 frames, and then sample 100,000 times (in that order).
  - You are encouraged to change any other code to improve performance and structure.
2. Implement the Add and Remove keyframe methods.
  - For this test, you do not need to sort the keyframes as they are pre-sorted. They are already being added in ascending order by time.
3. Implement FindNearestKeyframe.
  - Return the keyframe that is before the requested time value.
    - Example: Keyframe 1 has a time of 16 and keyframe 2 has a time of 20. If I call this function using the time of 19, it is expected to return keyframe 1.
  - The program should maintain acceptable performance when sampling many keyframes.
4. Write a method to interpolate between two arbitrary keyframes.
  - Return a new keyframe with the interpolated values.
  - If time allows, implement additional interpolation methods.

Additional stipulations
  - Do not include any additional namespaces or libraries
  - Do not modify the Main() method
  - If applicable, please write comments explaining the complexity and cost of your implemented algorithms.
  - To return your test send us your code in a zipped folder.
