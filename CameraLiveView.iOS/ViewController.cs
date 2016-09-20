using System;
using AVFoundation;
using Foundation;
using UIKit;

namespace CameraLiveView.iOS
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            AuthorizeCameraUse();

            InitCaptureSession();

            SetupPreviewLayer();


            session.StartRunning();
        }

        private AVCaptureSession session;

        void InitCaptureSession()
        {
            session = new AVCaptureSession();
            session.SessionPreset = AVCaptureSession.PresetHigh;

            // iPhone 7 Plusで全てのデバイスを列挙するなら
            // var deviceTypes = new[] { AVCaptureDeviceType.BuiltInDuoCamera, AVCaptureDeviceType.BuiltInWideAngleCamera, AVCaptureDeviceType.BuiltInTelephotoCamera};
            // var discoverySession = AVCaptureDeviceDiscoverySession.Create(deviceTypes, AVMediaType.Video, AVCaptureDevicePosition.Unspecified);
            // var devices = discoverySession.Devices;

            var defaultCamera = AVCaptureDevice.GetDefaultDevice(AVCaptureDeviceType.BuiltInWideAngleCamera, AVMediaType.Video, AVCaptureDevicePosition.Front);
            ConfigureCameraForDevice(defaultCamera);
            var input = AVCaptureDeviceInput.FromDevice(defaultCamera);
            session.AddInput(input);

            photoOutput = new AVCapturePhotoOutput();
            session.AddOutput(photoOutput);
        }

        private AVCaptureVideoPreviewLayer previewLayer;

        private AVCapturePhotoOutput photoOutput;

        void SetupPreviewLayer()
        {
            previewLayer = new AVCaptureVideoPreviewLayer(session);
            previewLayer.VideoGravity = AVLayerVideoGravity.ResizeAspect;
            previewLayer.Frame = LiveView.Bounds;
            LiveView.Layer.AddSublayer(previewLayer);
        }

        void ConfigureCameraForDevice(AVCaptureDevice device)
        {
            var error = new NSError();
            if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
            {
                device.LockForConfiguration(out error);
                device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
                device.UnlockForConfiguration();
            }
            else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
            {
                device.LockForConfiguration(out error);
                device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
                device.UnlockForConfiguration();
            }
            else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
            {
                device.LockForConfiguration(out error);
                device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
                device.UnlockForConfiguration();
            }
        }

        void AuthorizeCameraUse()
        {
            var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
            if (authorizationStatus != AVAuthorizationStatus.Authorized)
            {
                AVCaptureDevice.RequestAccessForMediaType(AVMediaType.Video, (accessGranted) => System.Diagnostics.Debug.WriteLine(accessGranted));
            }
        }
    }
}
