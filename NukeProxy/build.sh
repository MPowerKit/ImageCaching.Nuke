#!/bin/bash

echo "xcode build"
xcodebuild archive -destination "generic/platform=iOS" -project NukeProxy.xcodeproj -scheme NukeProxy -configuration Release -archivePath Output/Output-iphoneos SKIP_INSTALL=NO ENABLE_BITCODE=NO BUILD_LIBRARY_FOR_DISTRIBUTION=YES

xcodebuild archive -destination "generic/platform=iOS Simulator" -project NukeProxy.xcodeproj -scheme NukeProxy -configuration Release -archivePath Output/Output-iphonesimulator SKIP_INSTALL=NO ENABLE_BITCODE=NO BUILD_LIBRARY_FOR_DISTRIBUTION=YES

xcodebuild archive -destination "generic/platform=macOS,vairant=Mac Catalyst" -project NukeProxy.xcodeproj -scheme NukeProxy -configuration Release -archivePath Output/Output-maccatalyst SKIP_INSTALL=NO ENABLE_BITCODE=NO BUILD_LIBRARY_FOR_DISTRIBUTION=YES

echo "create xcframework"
xcodebuild -create-xcframework -archive Output/Output-iphoneos.xcarchive -framework NukeProxy.framework -archive Output/Output-iphonesimulator.xcarchive -framework NukeProxy.framework -output Output/NukeProxy-ios.xcframework

xcodebuild -create-xcframework -archive Output/Output-maccatalyst.xcarchive -framework NukeProxy.framework -output Output/NukeProxy-maccatalyst.xcframework

echo "sharpie bind"
sharpie bind --sdk=iphoneos17.5 --output="Output" --namespace="MPowerKit.NukeProxy" --scope="Output/NukeProxy.xcframework/ios-arm64/NukeProxy-ios.framework/Headers" "Output/NukeProxy-ios.xcframework/ios-arm64/NukeProxy.framework/Headers/NukeProxy-Swift.h"
