import Foundation
import UIKit
internal import Nuke
internal import NukeExtensions


@objc(ImagePipeline)
public class ImagePipeline : NSObject {
    
    @objc
    public static let shared = ImagePipeline()
    
    @objc
    public static func setupWithDataCache(){
        Nuke.ImagePipeline.shared = Nuke.ImagePipeline(configuration: .withDataCache)
    }
    
    @objc
    public func isCached(for url: URL) -> Bool {
        return Nuke.ImagePipeline.shared.cache.containsCachedImage(for: ImageRequest(url: url))
    }
    
    @objc
    public func getCachedImage(for url: URL) -> UIImage? {
        guard let cached = Nuke.ImagePipeline.shared.cache.cachedImage(for: ImageRequest(url: url)) else {
            return nil
        }
        
        return cached.image
    }
    
    @objc
    public func removeImageFromCache(for url: URL) {
        let imageRequest = ImageRequest(url: url)
        Nuke.ImagePipeline.shared.cache.removeCachedData(for: imageRequest)
        Nuke.ImagePipeline.shared.cache.removeCachedImage(for: imageRequest)
    }
    
    @objc
    public func removeAllCaches() {
        Nuke.ImagePipeline.shared.cache.removeAll()
    }
    
    @objc
    public func loadScaledImage(url: URL, scale: Float, onCompleted: @escaping (UIImage?, String) -> Void) {
        
        loadScaledImage(url: url, scale: scale, reloadIgnoringCachedData: false, onCompleted: onCompleted)
    }
    
    @objc
    public func loadScaledImage(url: URL, scale: Float, reloadIgnoringCachedData: Bool, onCompleted: @escaping (UIImage?, String) -> Void) {
        
        _ = Nuke.ImagePipeline.shared.loadImage(
            with: ImageRequest(
                urlRequest: URLRequest(
                    url: url,
                    cachePolicy: reloadIgnoringCachedData ?
                        .reloadIgnoringLocalAndRemoteCacheData :
                        .useProtocolCachePolicy
                ),
                userInfo: [.scaleKey : scale]
            ),
            completion: { result in
                switch result {
                case let .success(response):
                    onCompleted(response.image, "success")
                case let .failure(error):
                    onCompleted(nil, error.localizedDescription)
                }
            }
        )
    }

    @objc
    public func loadImage(url: URL, onCompleted: @escaping (UIImage?, String) -> Void) {
        
        Nuke.ImagePipeline.shared.loadImage(
            with: ImageRequest(
                url: url
            ),
            completion: { result in
                switch result {
                case let .success(response):
                    onCompleted(response.image, "success")
                case let .failure(error):
                    onCompleted(nil, error.localizedDescription)
                }
            }
        )
    }

    @MainActor @objc
    public func loadImage(url: URL, placeholder: UIImage?, errorImage: UIImage?, into: UIImageView, onCompleted: @escaping (UIImage?, String) -> Void) {
        
        loadImage(url: url, placeholder: placeholder, errorImage: errorImage, into: into, reloadIgnoringCachedData: false, onCompleted: onCompleted)
    }

    @MainActor @objc
    public func loadImage(url: URL, placeholder: UIImage?, errorImage: UIImage?, into: UIImageView, reloadIgnoringCachedData: Bool, onCompleted: @escaping (UIImage?, String) -> Void) {
        
        let options = ImageLoadingOptions(placeholder:placeholder, failureImage: errorImage)
        
        NukeExtensions.loadImage(
            with: ImageRequest(
                urlRequest: URLRequest(
                    url: url,
                    cachePolicy: reloadIgnoringCachedData ?
                        .reloadIgnoringLocalAndRemoteCacheData :
                        .useProtocolCachePolicy
                )
            ),
            options: options,
            into: into,
            completion: { result in
                switch result {
                case let .success(response):
                    onCompleted(response.image, "success")
                case let .failure(error):
                    onCompleted(nil, error.localizedDescription)
                }
            }
        )
    }

    @MainActor @objc
    public func loadImage(url: URL, imageIdKey: String, placeholder: UIImage?, errorImage: UIImage?, into: UIImageView, onCompleted: @escaping (UIImage?, String) -> Void) {
        
        loadImage(url: url, imageIdKey: imageIdKey, placeholder: placeholder, errorImage: errorImage, into: into, reloadIgnoringCachedData: false, onCompleted: onCompleted)
    }

    @MainActor @objc
    public func loadImage(url: URL, imageIdKey: String, placeholder: UIImage?, errorImage: UIImage?, into: UIImageView, reloadIgnoringCachedData: Bool, onCompleted: @escaping (UIImage?, String) -> Void) {
        
        let options = ImageLoadingOptions(placeholder: placeholder, failureImage: errorImage)

        NukeExtensions.loadImage(
            with: ImageRequest(
                urlRequest: URLRequest(
                    url: url,
                    cachePolicy: reloadIgnoringCachedData ?
                        .reloadIgnoringLocalAndRemoteCacheData :
                        .useProtocolCachePolicy
                ),
                userInfo: [.imageIdKey: imageIdKey ]
            ),
            options: options,
            into: into,
            completion: { result in
                switch result {
                case let .success(response):
                    onCompleted(response.image, "success")
                case let .failure(error):
                    onCompleted(nil, error.localizedDescription)
                }
            }
        )
    }

    @objc
    public func loadData(url: URL, onCompleted: @escaping (Data?, URLResponse?) -> Void) {
        
        loadData(url: url, imageIdKey: nil, reloadIgnoringCachedData: false, onCompleted: onCompleted)
    }

    @objc
    public func loadData(url: URL, imageIdKey: String?, reloadIgnoringCachedData: Bool, onCompleted: @escaping (Data?, URLResponse?) -> Void) {
        
        _ = Nuke.ImagePipeline.shared.loadData(
            with: ImageRequest(
                urlRequest: URLRequest(
                    url: url,
                    cachePolicy: reloadIgnoringCachedData ?
                        .reloadIgnoringLocalAndRemoteCacheData :
                        .useProtocolCachePolicy
                ),
                userInfo: imageIdKey == nil ? nil : [.imageIdKey: imageIdKey! ]
            ),
            completion: { result in
                switch result {
                case let .success(response):
                    onCompleted(response.data, response.response)
                case .failure(_):
                    onCompleted(nil, nil)
                }
            }
        )
    }
}

@objc(ImageCache)
public final class ImageCache: NSObject {
    
    @objc
    public static let shared = ImageCache()
    
    @objc
    public func removeAll() {
        Nuke.ImageCache.shared.removeAll()
    }
}

@objc(DataLoader)
public final class DataLoader: NSObject {
    
    @objc
    public static let shared = DataLoader()
    
    @objc
    public static let sharedUrlCache = Nuke.DataLoader.sharedUrlCache
}

@objc(Prefetcher)
public final class Prefetcher: NSObject {
 
    private var prefetcher: ImagePrefetcher
    
    @objc
    public override init() {
        prefetcher = ImagePrefetcher()
    }
    
    @objc
    public init(destination: Destination = .memoryCache) {
        prefetcher = ImagePrefetcher(destination: destination == .memoryCache ?
                                        ImagePrefetcher.Destination.memoryCache :
                                        ImagePrefetcher.Destination.diskCache)
    }
    
    @objc
    public init(destination: Destination = .memoryCache,
                maxConcurrentRequestCount: Int = 2) {
        prefetcher = ImagePrefetcher(destination: destination == .memoryCache ?
                                        ImagePrefetcher.Destination.memoryCache :
                                        ImagePrefetcher.Destination.diskCache,
                                     maxConcurrentRequestCount: maxConcurrentRequestCount)
    }
    
    @objc
    public func startPrefetching(with: [URL]) {
        prefetcher.startPrefetching(with: with)
    }
    
    @objc
    public func stopPrefetching(with: [URL]) {
        prefetcher.stopPrefetching(with: with)
    }
    
    @objc
    public func stopPrefetching() {
        prefetcher.stopPrefetching()
    }
    
    @objc
    public func pause() {
        prefetcher.isPaused = true
    }
    
    @objc
    public func unPause() {
        prefetcher.isPaused = false
    }
    
    @objc
    public enum Destination: Int {
        case memoryCache
        case diskCache
    }
}
