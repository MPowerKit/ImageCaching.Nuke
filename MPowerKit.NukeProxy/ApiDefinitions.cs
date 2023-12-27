using System;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace MPowerKit.NukeProxy
{
	// @interface DataLoader : NSObject
	[BaseType (typeof(NSObject))]
	interface DataLoader
	{
		// @property (readonly, nonatomic, strong, class) DataLoader * _Nonnull shared;
		[Static]
		[Export ("shared", ArgumentSemantic.Strong)]
		DataLoader Shared { get; }

		// @property (readonly, nonatomic, strong, class) NSURLCache * _Nonnull sharedUrlCache;
		[Static]
		[Export ("sharedUrlCache", ArgumentSemantic.Strong)]
		NSUrlCache SharedUrlCache { get; }
	}

	// @interface ImageCache : NSObject
	[BaseType (typeof(NSObject))]
	interface ImageCache
	{
		// @property (readonly, nonatomic, strong, class) ImageCache * _Nonnull shared;
		[Static]
		[Export ("shared", ArgumentSemantic.Strong)]
		ImageCache Shared { get; }

		// -(void)removeAll;
		[Export ("removeAll")]
		void RemoveAll ();
	}

	// @interface ImagePipeline : NSObject
	[BaseType (typeof(NSObject))]
	interface ImagePipeline
	{
		// @property (readonly, nonatomic, strong, class) ImagePipeline * _Nonnull shared;
		[Static]
		[Export ("shared", ArgumentSemantic.Strong)]
		ImagePipeline Shared { get; }

		// +(void)setupWithDataCache;
		[Static]
		[Export ("setupWithDataCache")]
		void SetupWithDataCache ();

		// -(BOOL)isCachedFor:(NSURL * _Nonnull)url __attribute__((warn_unused_result("")));
		[Export ("isCachedFor:")]
		bool IsCachedFor (NSUrl url);

		// -(UIImage * _Nullable)getCachedImageFor:(NSURL * _Nonnull)url __attribute__((warn_unused_result("")));
		[Export ("getCachedImageFor:")]
		[return: NullAllowed]
		UIImage GetCachedImageFor (NSUrl url);

		// -(void)removeImageFromCacheFor:(NSURL * _Nonnull)url;
		[Export ("removeImageFromCacheFor:")]
		void RemoveImageFromCacheFor (NSUrl url);

		// -(void)removeAllCaches;
		[Export ("removeAllCaches")]
		void RemoveAllCaches ();

        // -(void)loadScaledImageWithUrl:(NSURL * _Nonnull)url scale:(float)scale onCompleted:(void (^ _Nonnull)(UIImage * _Nullable, NSString * _Nonnull))onCompleted;
        [Export("loadScaledImageWithUrl:scale:onCompleted:")]
        void LoadScaledImageWithUrl(NSUrl url, float scale, Action<UIImage, NSString> onCompleted);

        // -(void)loadImageWithUrl:(NSURL * _Nonnull)url onCompleted:(void (^ _Nonnull)(UIImage * _Nullable, NSString * _Nonnull))onCompleted;
        [Export ("loadImageWithUrl:onCompleted:")]
		void LoadImageWithUrl (NSUrl url, Action<UIImage, NSString> onCompleted);

		// -(void)loadImageWithUrl:(NSURL * _Nullable)url placeholder:(UIImage * _Nullable)placeholder errorImage:(UIImage * _Nullable)errorImage into:(UIImageView * _Nonnull)into;
		[Export ("loadImageWithUrl:placeholder:errorImage:into:")]
		void LoadImageWithUrl ([NullAllowed] NSUrl url, [NullAllowed] UIImage placeholder, [NullAllowed] UIImage errorImage, UIImageView into);

		// -(void)loadImageWithUrl:(NSURL * _Nullable)url imageIdKey:(NSString * _Nonnull)imageIdKey placeholder:(UIImage * _Nullable)placeholder errorImage:(UIImage * _Nullable)errorImage into:(UIImageView * _Nonnull)into;
		[Export ("loadImageWithUrl:imageIdKey:placeholder:errorImage:into:")]
		void LoadImageWithUrl ([NullAllowed] NSUrl url, string imageIdKey, [NullAllowed] UIImage placeholder, [NullAllowed] UIImage errorImage, UIImageView into);

		// -(void)loadDataWithUrl:(NSURL * _Nullable)url onCompleted:(void (^ _Nonnull)(NSData * _Nullable, NSURLResponse * _Nullable))onCompleted;
		[Export ("loadDataWithUrl:onCompleted:")]
		void LoadDataWithUrl ([NullAllowed] NSUrl url, Action<NSData, NSUrlResponse> onCompleted);

		// -(void)loadDataWithUrl:(NSURL * _Nullable)url imageIdKey:(NSString * _Nullable)imageIdKey reloadIgnoringCachedData:(BOOL)reloadIgnoringCachedData onCompleted:(void (^ _Nonnull)(NSData * _Nullable, NSURLResponse * _Nullable))onCompleted;
		[Export ("loadDataWithUrl:imageIdKey:reloadIgnoringCachedData:onCompleted:")]
		void LoadDataWithUrl ([NullAllowed] NSUrl url, [NullAllowed] string imageIdKey, bool reloadIgnoringCachedData, Action<NSData, NSUrlResponse> onCompleted);
	}

	// @interface Prefetcher : NSObject
	[BaseType (typeof(NSObject))]
	interface Prefetcher
	{
		// -(instancetype _Nonnull)initWithDestination:(enum Destination)destination __attribute__((objc_designated_initializer));
		[Export ("initWithDestination:")]
		[DesignatedInitializer]
		NativeHandle Constructor (Destination destination);

		// -(instancetype _Nonnull)initWithDestination:(enum Destination)destination maxConcurrentRequestCount:(NSInteger)maxConcurrentRequestCount __attribute__((objc_designated_initializer));
		[Export ("initWithDestination:maxConcurrentRequestCount:")]
		[DesignatedInitializer]
		NativeHandle Constructor (Destination destination, nint maxConcurrentRequestCount);

		// -(void)startPrefetchingWith:(NSArray<NSURL *> * _Nonnull)with;
		[Export ("startPrefetchingWith:")]
		void StartPrefetchingWith (NSUrl[] with);

		// -(void)stopPrefetchingWith:(NSArray<NSURL *> * _Nonnull)with;
		[Export ("stopPrefetchingWith:")]
		void StopPrefetchingWith (NSUrl[] with);

		// -(void)stopPrefetching;
		[Export ("stopPrefetching")]
		void StopPrefetching ();

		// -(void)pause;
		[Export ("pause")]
		void Pause ();

		// -(void)unPause;
		[Export ("unPause")]
		void UnPause ();
	}
}
