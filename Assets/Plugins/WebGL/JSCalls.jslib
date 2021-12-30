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

  JSCreateUnitRequest: function (json, matchId) {
        ReactUnityWebGL.CreateUnitRequest(Pointer_stringify(json), matchId);
  },

  JSSearchGame: function (walletId, playerData) {
        ReactUnityWebGL.SearchGame(walletId, playerData);
  },
});