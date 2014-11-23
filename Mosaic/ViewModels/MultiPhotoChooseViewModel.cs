using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Caliburn.Micro;

using Microsoft.Xna.Framework.Media;

using Mosaic.Model;
using Mosaic.Views;

using NameIt.Common;

namespace Mosaic.ViewModels
{
    public class MultiPhotoChooseViewModel : MosaicViewModel
    {
        private readonly IEventAggregator eventAggregator;
        private AlbumSelectMode albumSelectMode = AlbumSelectMode.None;

        public MultiPhotoChooseViewModel(BackgroundImageBrush backgroundImageBrush, INavigationService navigationService, ILog logger, IEventAggregator eventAggregator) : base(backgroundImageBrush, navigationService, logger)
        {
            this.eventAggregator = eventAggregator;
            AvailableAlbums = new ObservableCollection<ImageAlbum>();

            SelectImagesText = "select";
            SelectImageIcon = new Uri("Assets/check.png", UriKind.Relative);
        }

        private ObservableCollection<ImageAlbum> availableAlbums;

        public ObservableCollection<ImageAlbum> AvailableAlbums
        {
            get { return availableAlbums; }
            set
            {
                availableAlbums = value;
                NotifyOfPropertyChange(() => AvailableAlbums);
            }
        }

        private MultiPhotoChooseView view;

        protected override void OnViewLoaded(object view)
        {
            this.view = view as MultiPhotoChooseView;
            base.OnViewLoaded(view);
            LoadAvailableAlbums();
            albumSelectMode = AlbumSelectMode.AlbumSelection;
            this.view.AvailableAlbums.IsCheckModeActive = false;
        }

        public void LoadAvailableAlbums()
        {
            foreach (MediaSource source in MediaSource.GetAvailableMediaSources())
            {
                if (source.MediaSourceType == MediaSourceType.LocalDevice)
                {
                    var mediaLibrary = new MediaLibrary(source);

                    PictureAlbumCollection albums = mediaLibrary.RootPictureAlbum.Albums;
                
                    foreach (PictureAlbum album in albums)
                    {
                        var imageAlbum = new ImageAlbum();
                        imageAlbum.Name = album.Name;
                        if (album.Pictures.Count > 0)
                        {
                            var albumImage = new BitmapImage();
                            albumImage.SetSource(album.Pictures[0].GetThumbnail());
                            imageAlbum.Image = albumImage;
                        }
                        AvailableAlbums.Add(imageAlbum);
                    }
                }
            }
        }

        public void LoadPicturesOfAlbums(string albumName)
        {
            albumSelectMode = AlbumSelectMode.PictureSelection;
            IsAppBarVisible = true;
            AvailableAlbums.Clear();
            this.view.AvailableAlbums.ItemTemplate = this.view.LayoutRoot.Resources["WrapModeDataTemplate"] as DataTemplate;
            this.view.AvailableAlbums.IsCheckModeActive = true;

            foreach (MediaSource source in MediaSource.GetAvailableMediaSources())
            {
                if (source.MediaSourceType == MediaSourceType.LocalDevice)
                {
                    var mediaLibrary = new MediaLibrary(source);

                    PictureAlbum pictureAlbum = mediaLibrary.RootPictureAlbum.Albums.SingleOrDefault(al => al.Name == albumName);
                    if (pictureAlbum != null)
                    {
                        foreach (Picture picture in pictureAlbum.Pictures)
                        {
                            var imageAlbum = new ImageAlbum();
                            var bitmapImage = new BitmapImage();
                            bitmapImage.SetSource(picture.GetThumbnail());
                            imageAlbum.Image = bitmapImage;

                            var newBitmap = new BitmapImage();
                            newBitmap.SetSource(picture.GetImage());
                            imageAlbum.WritableBitmap = new WriteableBitmap(newBitmap);
                            imageAlbum.WritableBitmap.Resize(60, 60, WriteableBitmapExtensions.Interpolation.Bilinear);
                            
                            picture.Dispose();

                            AvailableAlbums.Add(imageAlbum);
                        }
                    }
                }
            }
        }

        public void SelectImages()
        {
            if (albumSelectMode == AlbumSelectMode.PictureSelection)
            {
                var selectedItemsEvent = new SelectedItemsEvent();
                selectedItemsEvent.Items = view.AvailableAlbums.CheckedItems.Cast<ImageAlbum>().ToList();
                eventAggregator.Publish(selectedItemsEvent);
                navigationService.GoBack();
            }
        }

        private ImageAlbum selectedAlbum;

        public ImageAlbum SelectedAlbum
        {
            get { return selectedAlbum; }
            set
            {
                if (albumSelectMode == AlbumSelectMode.AlbumSelection)
                {
                    selectedAlbum = value;
                    NotifyOfPropertyChange(() => SelectedAlbum);
                    LoadPicturesOfAlbums(selectedAlbum.Name);
                }
            }
        }

        private bool isAppBarVisible;

        public bool IsAppBarVisible
        {
            get { return isAppBarVisible; }
            set
            {
                isAppBarVisible = value;
                NotifyOfPropertyChange(() => IsAppBarVisible);
            }
        }

        private string selectImagesText;

        public string SelectImagesText
        {
            get { return selectImagesText; }
            set
            {
                selectImagesText = value;
                NotifyOfPropertyChange(() => SelectImagesText);
            }
        }

        private Uri selectImageIcon;

        public Uri SelectImageIcon
        {
            get { return selectImageIcon; }
            set
            {
                selectImageIcon = value;
                NotifyOfPropertyChange(() => SelectImageIcon);
            }
        }
    }
}