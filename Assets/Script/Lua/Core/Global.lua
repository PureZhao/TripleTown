_G.CSE = CS.UnityEngine
_G.IS_EDITOR = CSE.Application.isEditor

_G.logInfo = print

function bind(func, this, ...)
    if this then
        return function (...)
            func(this, ...)
        end
    else
        return function (...)
            func(...)
        end
    end
end

function extendClass(target, base)
    if base then
        for k, v in pairs(base) do
            target[k] = v
        end
    end
end