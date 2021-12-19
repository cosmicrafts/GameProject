mergeInto(LibraryManager.library, {
  // Create a new function with the same name as
  // the event listeners name and make sure the
  // parameters match as well.

  JSSaveScore: function (score) {
        ReactUnityWebGL.SaveScore(score);
  },

  JSSendGameData: function (json, matchId) {
        ReactUnityWebGL.SendGameData(Pointer_stringify(json), matchId);
  },

  JSGetGameData: function (matchId) {
      return ReactUnityWebGL.GetGameData(matchId);
  },

  JSCreateGame: function (walletId) {
    return ReactUnityWebGL.CreateGame(walletId);
  },

  JSSearchGame: function (walletId) {
      return ReactUnityWebGL.SearchGame(walletId);
  },
});