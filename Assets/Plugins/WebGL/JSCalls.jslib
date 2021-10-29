mergeInto(LibraryManager.library, {
  // Create a new function with the same name as
  // the event listeners name and make sure the
  // parameters match as well.

    SaveScore: function (score) {

        ReactUnityWebGL.SaveScore(score);
  },

    SendJson: function (json) {

        ReactUnityWebGL.SendJson(Pointer_stringify(json));
        console.log(Pointer_stringify(json));
  },
});