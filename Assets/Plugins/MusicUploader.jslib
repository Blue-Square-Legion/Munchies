class BeatBeat {
	offlineContext;
	buffer;

	constructor(
		context,
		name,
		filterFrequency = 100,
		peakGain = 15,
		threshold = 0.8,
		sampleSkip = 350,
		minAnimationTime = 0.4
	) {
        this.context = new AudioContext();
        this.name = name;
        this.filterFrequency = filterFrequency;
        this.peakGain = peakGain;
        this.threshold = threshold;
        this.sampleSkip = sampleSkip;
        this.minAnimationTime = minAnimationTime;
        this.songData = [];
	}

	async load() {
		const resp = await fetch(this.name);
		const file = await resp.arrayBuffer();
		return this.context.decodeAudioData(file, this.analyze);
	}

	async loadFile(arrayBuffer){
		return this.context.decodeAudioData(file, this.analyze);
	}


	async analyze(buffer) {
		this.offlineContext = new OfflineAudioContext(1, buffer.length, buffer.sampleRate)
		const source = this.offlineContext.createBufferSource()
		source.buffer = buffer

		const filter = this.offlineContext.createBiquadFilter()
		filter.type = "bandpass"
		filter.frequency.value = this.filterFrequency
		filter.Q.value = 1

		const filter2 = this.offlineContext.createBiquadFilter()
		filter2.type = "peaking"
		filter2.frequency.value = this.filterFrequency
		filter2.Q.value = 1
		filter2.gain.value = this.peakGain

		source.connect(filter2)
		filter2.connect(filter)
		filter.connect(this.offlineContext.destination)
		source.start()
		const buffer = await this.offlineContext.startRendering()

		const data = buffer.getChannelData(0)

		this.songData = []
		for (let i = 0; i < data.length; ++i) {
			if (data[i] > this.threshold) {
				const time = i / buffer.sampleRate
				const previousTime = this.songData.length
					? this.songData[this.songData.length - 1].time
					: 0
				if (time - previousTime > this.minAnimationTime) {
					this.songData.push({
						data: data[i],
						time
					})
				}
			}
			i += this.sampleSkip
		}
    return this.songData;
	}

  
}


const ImageUploaderPlugin = {
  
  ImageUploaderCaptureClick: function(callback) {
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