const ImageUploaderPlugin = {
  
  ImageUploaderCaptureClick: function(name) {
    const UPLOAD_ID = "uploader";
    const GAMEOBJECT_NAME = "Upload"

    const ACCEPT_TYPES = ["audio/wav", "audio/ogg","audio/mpeg"].join(', ');
       
    
    const unityCanvas = document.getElementById('unity-canvas');

    if (!document.getElementById(UPLOAD_ID)) {
      let fileInput = document.createElement('input');
      fileInput.style.display = 'none';      
      fileInput.setAttribute('type', 'file');
      fileInput.setAttribute('id', UPLOAD_ID);
      fileInput.setAttribute('accept', ACCEPT_TYPES);
      
      fileInput.onclick = function (event) {
        this.value = null;
      };
      fileInput.onchange = function (event) {
        const file = event.target.files[0];
        const data = {	//JSON data
          url: URL.createObjectURL(file),
          type: file.type,
          name: file.name,
        };

        SendMessage(GAMEOBJECT_NAME, 'FileSelected', JSON.stringify(data));
      }
      document.body.appendChild(fileInput);
    }

    const OpenFileDialog = function() {
      document.getElementById(UPLOAD_ID).click();
      unityCanvas.removeEventListener('click', OpenFileDialog);
    };

    unityCanvas.addEventListener('click', OpenFileDialog, false);
  }
};
mergeInto(LibraryManager.library, ImageUploaderPlugin);