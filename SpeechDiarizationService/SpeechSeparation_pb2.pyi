from google.protobuf.internal import containers as _containers
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from typing import ClassVar as _ClassVar, Iterable as _Iterable, Mapping as _Mapping, Optional as _Optional, Union as _Union

DESCRIPTOR: _descriptor.FileDescriptor

class SeparationData(_message.Message):
    __slots__ = ("start", "end", "speaker_name")
    START_FIELD_NUMBER: _ClassVar[int]
    END_FIELD_NUMBER: _ClassVar[int]
    SPEAKER_NAME_FIELD_NUMBER: _ClassVar[int]
    start: float
    end: float
    speaker_name: str
    def __init__(self, start: _Optional[float] = ..., end: _Optional[float] = ..., speaker_name: _Optional[str] = ...) -> None: ...

class ResponseData(_message.Message):
    __slots__ = ("file_name", "separation_datas")
    FILE_NAME_FIELD_NUMBER: _ClassVar[int]
    SEPARATION_DATAS_FIELD_NUMBER: _ClassVar[int]
    file_name: str
    separation_datas: _containers.RepeatedCompositeFieldContainer[SeparationData]
    def __init__(self, file_name: _Optional[str] = ..., separation_datas: _Optional[_Iterable[_Union[SeparationData, _Mapping]]] = ...) -> None: ...

class RequestData(_message.Message):
    __slots__ = ("file_path",)
    FILE_PATH_FIELD_NUMBER: _ClassVar[int]
    file_path: str
    def __init__(self, file_path: _Optional[str] = ...) -> None: ...
