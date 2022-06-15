mergeInto(LibraryManager.library, {
  // Create a new function with the same name as
  // the event listeners name and make sure the
  // parameters match as well.

      JSSaveScore: function (score) {
            ReactUnityWebGL.SaveScore(score);
      },

      JSSavePlayerConfig: function (json) {
            ReactUnityWebGL.SavePlayerConfig(Pointer_stringify(json));
      },

      JSSavePlayerCharacter: function (json) {
            ReactUnityWebGL.SavePlayerCharacter(Pointer_stringify(json));
      },

      JSSendMasterData: function (json) {
            ReactUnityWebGL.SendMasterData(Pointer_stringify(json));
      },

      JSSendClientData: function (json) {
            ReactUnityWebGL.SendClientData(Pointer_stringify(json));
      },

      JSExitGame: function () {
            ReactUnityWebGL.ExitGame();
      },

      JSSearchGame: function (json) {
            ReactUnityWebGL.SearchGame(Pointer_stringify(json));
      },

      JSCancelGame: function (gameId) {
            ReactUnityWebGL.CancelGame(gameId);
      },

     JSLoginPanel: function (json) {
            ReactUnityWebGL.JSLoginPanel(Pointer_stringify(json));

      },

      JSWalletsLogin: function (walletName) {
            ReactUnityWebGL.JSWalletsLogin(Pointer_stringify(walletName));
      },

      RequestAnvilConnect: function(anvilUrl) {
        dispatchReactUnityEvent("RequestIcpConnect", anvilUrl);
    },
    
    CheckAnvilConnection: function(anvilIndex) {
        dispatchReactUnityEvent("CheckAnvilConnection", anvilUrl);
    },
    
    GetAnvilNfts : function (anvilUrl) 
    {
        dispatchReactUnityEvent("GetAnvilNfts", anvilUrl);
    }


});