mergeInto(LibraryManager.library, {
  // Create a new function with the same name as
  // the event listeners name and make sure the
  // parameters match as well.

  JSSaveScore: function (score) {
        ReactUnityWebGL.SaveScore(score);
  },

  JSSendMasterData: function (json) {
        ReactUnityWebGL.SendMasterData(Pointer_stringify(json));
  },

  JSSendClientData: function (json) {
        ReactUnityWebGL.SendClientData(Pointer_stringify(json));
  },

  JSSearchGame: function (json) {
        ReactUnityWebGL.SearchGame(Pointer_stringify(json));
  },
});