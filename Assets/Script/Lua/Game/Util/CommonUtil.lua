local CommonUtil = {}

function CommonUtil.GenerateMatrix(h, w, fill)
    local m = {}
    for i = 1, h do
        m[i] = {}
        for j = 1, w do
            m[i][j] = fill
        end
    end
    return m
end

return CommonUtil