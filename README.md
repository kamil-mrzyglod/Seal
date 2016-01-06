# Seal
A custom [Fody](https://github.com/Fody/Fody) weaver marking all non-virtual(abstract, non-sealed) types as sealed by default.

Inspired by an excellent series of posts by Joe Duffy - http://joeduffyblog.com/2015/11/03/blogging-about-midori/

## Why?
As per Jeffrey Richter(http://codebetter.com/patricksmacchia/2008/01/05/rambling-on-the-sealed-keyword/) - why you want to mark all your classes as 'sealed' by default:

- Versioning: When a class is originally sealed, it can change to unsealed in the future without breaking compatibility. (…)

- Performance: (…) if the JIT compiler sees a
call to a virtual method using a sealed types, the JIT compiler can
produce more efficient code by calling the method non-virtually.(…)

- Security and Predictability:
A class must protect its own state and not allow itself to ever become
corrupted. When a class is unsealed, a derived class can access and
manipulate the base class’s state if any data fields or methods that
internally manipulate fields are accessible and not private.(…)


## How to install?
    Install-Package Seal.Fody

## Usage
Usage is super simple, just add new weaver to `FodyWeavers.xml`(if you install package from NuGet it will be done automatically).

Additionally you can use `[LeaveUnsealed]` attribute to prevent `Seal` from marking type as sealed:

```
[LeaveUnsealed]
public class DontSealMe
{
}
```
